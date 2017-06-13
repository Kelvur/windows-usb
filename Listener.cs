
using System;

namespace WindowsUsb {
	
	public class UsbListener {
		
		private string usbHash;
		private Func<string> callback;
		
		public UsbListener(string usbHash, Func<string> callback){
			if(usbHash == null){
				throw new System.ArgumentException("Argument cannot be null.", "usbHash");
			}
			if(callback == null){
				throw new System.ArgumentException("Argument cannot be null.", "callback");
			}
			this.usbHash = usbHash;
			this.callback = callback;
		}
		
		public void start(){
			bool connected;
			bool alreadyNotify = false;
			while(true){
				connected = false;
				foreach(UsbStore usb in UsbStoreProvider.getAllUsbStore()){
					if(usbHash == usb.getHash()){
						connected = true;
						if(!alreadyNotify){
							alreadyNotify = true;
							callback();
						}
					}
				}
				if(!connected){
					alreadyNotify = false;
				}
			}
		}
		
	}
	
}