// Theme Switcher: shows a random, blurred library backdrop behind pages that have none of their own.
// Active only when the stylesheet sets --ts-random-bg: 1 (the "random background" setting). Pages with a backdrop of
// their own, such as item pages, keep it: the stylesheet hides this layer while jellyfin-web shows one.
(function () {
    'use strict';

    if (window.ThemeSwitcherLoaded) {
        return;
    }
    window.ThemeSwitcherLoaded = true;

    const POOL_KEY = 'themeSwitcher.backdropPool';
    const POOL_SIZE = 30;
    const CLASS_NAME = 'ts-random-bg';

    let layers = null;
    let activeLayer = 0;
    let lastRoute = null;
    let lastUrl = null;
    let poolPromise = null;
    let token = 0;

    function isEnabled() {
        return getComputedStyle(document.documentElement).getPropertyValue('--ts-random-bg').trim() === '1';
    }

    function ensureLayers() {
        if (layers) {
            return layers;
        }

        const container = document.createElement('div');
        container.id = 'tsRandomBackdrop';
        layers = [document.createElement('div'), document.createElement('div')];
        layers.forEach(function (layer) { container.appendChild(layer); });
        document.body.insertBefore(container, document.body.firstChild);
        return layers;
    }

    function apiClient() {
        const client = window.ApiClient;
        return client && client.getCurrentUserId && client.getCurrentUserId() ? client : null;
    }

    // A pool of backdrops per session, so a page change costs no request.
    function loadPool(client) {
        if (poolPromise) {
            return poolPromise;
        }

        try {
            const cached = JSON.parse(sessionStorage.getItem(POOL_KEY) || 'null');
            if (cached && cached.userId === client.getCurrentUserId() && cached.items.length) {
                poolPromise = Promise.resolve(cached.items);
                return poolPromise;
            }
        } catch (e) {
            // sessionStorage can be unavailable, the pool is then fetched every time the page loads
        }

        poolPromise = client.getItems(client.getCurrentUserId(), {
            SortBy: 'Random',
            Limit: POOL_SIZE,
            Recursive: true,
            IncludeItemTypes: 'Movie,Series',
            ImageTypes: 'Backdrop',
            EnableTotalRecordCount: false,
            // Same as jellyfin-web's own global backdrops
            MaxOfficialRating: 'PG-13'
        }).then(function (result) {
            const items = result.Items
                .filter(function (item) { return item.BackdropImageTags && item.BackdropImageTags.length; })
                .map(function (item) { return { id: item.Id, tag: item.BackdropImageTags[0] }; });
            try {
                sessionStorage.setItem(POOL_KEY, JSON.stringify({ userId: client.getCurrentUserId(), items: items }));
            } catch (e) {
                // see above
            }
            return items;
        }).catch(function () {
            poolPromise = null;
            return [];
        });

        return poolPromise;
    }

    function show() {
        const client = apiClient();
        if (!client) {
            return;
        }

        const myToken = ++token;
        loadPool(client).then(function (items) {
            if (myToken !== token || !items.length) {
                return;
            }

            let url;
            do {
                const pick = items[Math.floor(Math.random() * items.length)];
                // It is blurred and dimmed, so there is no point in a big or high quality image.
                url = client.getScaledImageUrl(pick.id, { type: 'Backdrop', tag: pick.tag, maxWidth: 1280, quality: 60 });
            } while (url === lastUrl && items.length > 1);

            const img = new Image();
            img.onload = function () {
                if (myToken !== token) {
                    return;
                }

                const pair = ensureLayers();
                const next = 1 - activeLayer;
                pair[next].style.backgroundImage = 'url("' + url + '")';
                pair[next].classList.add('on');
                pair[activeLayer].classList.remove('on');
                activeLayer = next;
                lastUrl = url;
            };
            img.src = url;
        });
    }

    // Tab switches and filters change only the query, which isn't a new page.
    function route() {
        return location.hash.split('?')[0];
    }

    function update() {
        const enabled = isEnabled();
        document.documentElement.classList.toggle(CLASS_NAME, enabled);
        if (!enabled) {
            return;
        }

        if (route() !== lastRoute || !lastUrl) {
            lastRoute = route();
            show();
        }
    }

    window.addEventListener('hashchange', update);
    window.addEventListener('load', update);

    // The API client and the user are not there yet on the first call (and not at all on the login page).
    let attempts = 0;
    const timer = setInterval(function () {
        attempts++;
        update();
        if (lastUrl || attempts > 60) {
            clearInterval(timer);
        }
    }, 500);
})();
