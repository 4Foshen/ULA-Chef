mergeInto(LibraryManager.library, {
    
    // Получение данных о пользователе
    Telegram_GetUserData: function() {
        console.log("Getting user data");
        let tg = window.Telegram.WebApp;
        
        var dataStr = JSON.stringify(tg.initDataUnsafe.user);
        var bufferSize = lengthBytesUTF8(dataStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(dataStr, buffer, bufferSize);
        
        return buffer;
    }
});