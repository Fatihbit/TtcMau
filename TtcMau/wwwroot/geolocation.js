window.getGeolocation = function (dotNetHelper) {
    console.log("getGeolocation called"); // Debugging: Check if this function is called
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(position => {
            console.log("Geolocation success", position.coords.latitude, position.coords.longitude); // Debugging: Check if geolocation is successful
            dotNetHelper.invokeMethodAsync('ReceiveGeolocation', position.coords.latitude, position.coords.longitude);
        }, error => {
            console.error(error);
        });
    } else {
        console.error("Geolocation is not supported by this browser.");
    }
};
