mergeInto(LibraryManager.library, {
    
    Telegram_GetUserData: function() {
        console.log("Getting user data");
        let tg = window.Telegram.WebApp;
        
        var dataStr = JSON.stringify(tg.initDataUnsafe.user);
        var bufferSize = lengthBytesUTF8(dataStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(dataStr, buffer, bufferSize);
        
        return buffer;
    },
   
         Telegram_SendMessage: function(messagePtr) {
             // Преобразуем указатель в строку
             var message = UTF8ToString(messagePtr);
             console.log("Sending message: " + message);
             // Отправляем сообщение боту через WebApp.sendData
             window.Telegram.WebApp.sendData(message);
         }
});