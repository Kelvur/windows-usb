
using System;
using System.Collections.Generic;
using System.Management;
using System.Text.RegularExpressions;


namespace WindowsUsb {
	
	class UsbStoreProvider {
		
		private const string QUERY_USB_DISK = "SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'";
		private const string QUERY_DISK_TO_PARTITION = "ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{0}'}} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
		private const string QUERY_LOGICAL_DISK_TO_PARTITION = "ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{0}'}} WHERE AssocClass = Win32_LogicalDiskToPartition";
		
		private const string KEY_DISK_DEVICE_ID = "DeviceID";
		private const string KEY_MODEL = "Model";
		private const string KEY_PATH = "Name";
		private const string KEY_PARTITION_DEVICE_ID = "DeviceID";
		private const string KEY_SERIAL_NUMBER = "SerialNumber";
		private const string KEY_SIZE = "Size";
		
		private UsbStore buildUsbStore(ManagementObject search){
			UsbStore usb = new UsbStore();
			usb.setModel(assingModel(search));
			usb.setSerialNumber(assingSerialNumber(search));
			usb.setSize(assingSize(search));
			
			string diskDeviceId = search[KEY_DISK_DEVICE_ID].ToString();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(String.Format(QUERY_DISK_TO_PARTITION, diskDeviceId));
			foreach(ManagementObject searchPartition in searcher.Get()){
				assingPartitionInfo(usb, searchPartition);
			}
			return usb;
		}
		
		public UsbStore[] getAllUsbStore(){
			List<UsbStore> usbCollection = new List<UsbStore>();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(QUERY_USB_DISK);
			foreach(ManagementObject search in searcher.Get()){
				usbCollection.Add(buildUsbStore(search));
			}
			return usbCollection.ToArray();
		}
		
		private string assingModel(ManagementObject search){
			return search[KEY_MODEL].ToString();
		}
		
		private void assingPartitionInfo(UsbStore usb, ManagementObject searchPartition){
			string partitionDeviceId = searchPartition[KEY_PARTITION_DEVICE_ID].ToString();
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(String.Format(QUERY_LOGICAL_DISK_TO_PARTITION, partitionDeviceId));
			foreach(ManagementObject search in searcher.Get()){
				usb.setPath(assingPath(search));
			}
		}
		
		private string assingPath(ManagementObject search){
			return search[KEY_PATH].ToString();
		}
		
		private string assingSerialNumber(ManagementObject search){
			return search[KEY_SERIAL_NUMBER].ToString();
		}
		
		private long assingSize(ManagementObject search){
			return Convert.ToInt64(search[KEY_SIZE]);
		}
		
		
	}
	
}