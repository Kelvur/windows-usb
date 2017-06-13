
using System;
using System.Security.Cryptography;
using System.Text;

namespace WindowsUsb {
	
	class UsbStore {
		
		private string model = null;
		private string path = null;
		private string serialNumber = null;
		private long size = 0;
		
		// ====== GETTER ======
		public string getHash(){
			using(MD5 hash = MD5.Create()){
				byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(model + serialNumber));
				StringBuilder builder = new StringBuilder();
				for(int i = 0; i < data.Length; i++){
					builder.Append(data[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}
		public string getPath(){
			return path;
		}
		public string getSerialNumber(){
			return serialNumber;
		}
		public string getModel(){
			return model;
		}
		public long getSize(){
			return size;
		}
		
		// ====== SETTER ======
		public void setPath(string path){
			this.path = path;
		}
		public void setSerialNumber(string serialNumber){
			this.serialNumber = serialNumber;
		}
		public void setModel(string model){
			this.model = model;
		}
		public void setSize(long size){
			this.size = size;
		}
		
		public override string ToString(){
			return String.Format("{0}:\n-Model: {1}\n-Path: {2}\n-SerialNumber: {3}\n-Size: {4} Bytes\n-Hash: {5}",this.GetType().FullName, model, path, serialNumber, size, getHash());
		}
		
	}
}
	
