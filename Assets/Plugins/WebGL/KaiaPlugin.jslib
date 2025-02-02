// KaiaPlugin.jslib
mergeInto(LibraryManager.library, {
    ConnectWallet: function() {
        window.ConnectWallet();
    },
    
    GetConnectedAddress: function() {
        var address = window.GetConnectedAddress();
        var bufferSize = lengthBytesUTF8(address) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(address, buffer, bufferSize);
        return buffer;
    },

    MintToken: function(amount) {
        window.MintToken(amount);
    },

    GetBalance: function() {
        window.GetBalance();
    }
});