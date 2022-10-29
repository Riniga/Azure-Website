async function PageLoaded()
{
    if (hasServiceWorkerSupport()) {
        if (Debug) console.log("Service Worker Supported");
        const swRegistration = await registerServiceWorker();
        const permission = await requestNotificationPermission();
    }
}

function hasServiceWorkerSupport()
{
    if (!('serviceWorker' in navigator)) throw new Error('No Service Worker support!')
    if (!('PushManager' in window)) throw new Error('No Push API Support!')
    return true;
}

const registerServiceWorker = async () => 
{
    const swRegistration = await navigator.serviceWorker.register('/serviceWorker.js'); 
    return swRegistration;
}

const requestNotificationPermission = async () => 
{
    const permission = await window.Notification.requestPermission();
    if(permission !== 'granted') throw new Error('Permission not granted for Notification');
}

PageLoaded();

