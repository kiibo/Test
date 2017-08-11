using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zebra
{
    public class ErrorCode
    {
        public string GetError(int code)
        {
            string error = "";
            switch (code)
            {
                case -1:
                    error= "Mechanical error";
                    break;
                case 1:
                    error = "Indicates a broken ribbon";
                    break;
                case 2:
                    error = "Print head temperature is too high";
                    break;
                case 3:
                    error = "Mechanical error";
                    break;
                case 4:
                    error = "Printer is out of cards,or unable to to feed the card";
                    break;
                case 5:
                    error = "Unable to encode magnetic or smart card encode";
                    break;
                case 6:
                    error = "Unable to encode the card because it is not in the encoder";
                    break;
                case 7:
                    error = "Print head is up";
                    break;
                case 8:
                    error = "Out of ribbon";
                    break;
                case 9:
                    error = "Ribbon needs to be removed";
                    break;
                case 10:
                    error = "Wrong number of parameters or a value is incorrect";
                    break;
                case 11:
                    error = "Invalid coordinates while trying to draw a barcode or graphics";
                    break;
                case 12:
                    error = "Undefined barcode type";
                    break;
                case 13:
                    error = "Text for magnetic encoding or bar code drawing is invalid";
                    break;
                case 14:
                    error = "Invalid command";
                    break;
                case 20:
                    error = "Syntax error in the barcode command or parameters";
                    break;
                case 21:
                    error = "General text data error";
                    break;
                case 22:
                    error = "Syntax error in the graphic command data";
                    break;
                case 30:
                    error = "Unable to initialize the graphics buffer";
                    break;
                case 31:
                    error = "Graphic object to be drawn exceeds the X range";
                    break;
                case 32:
                    error = "Graphic object to be drawn exceeds the Y range";
                    break;
                case 33:
                    error = "Graphic data checksum error";
                    break;
                case 34:
                    error = "Data time-out error, usually happens when the USB cable is taken out while printing";
                    break;
                case 35:
                    error = "Incorrect ribbon installed";
                    break;
                case 40:
                    error = "Invalid magnetic encoding data";
                    break;
                case 41:
                    error = "Error while encoding a magnetic stripe";
                    break;
                case 42:
                    error = "Error while reading a magnetic stripe";
                    break;
                case 44:
                    error = "Magnetic encoder not responding";
                    break;
                case 45:
                    error = "Magnetic encoder is missing or the card is jammed before reaching the encoder";
                    break;
                case 47:
                    error = "Error while trying to flip the card";
                    break;
                case 48:
                    error = "Feeder Cover Lid is open (P110 and P120 only)";
                    break;
                case 49:
                    error = "Error while trying to encode on a magnetic stripe";
                    break;
                case 50:
                    error = "Magnetic encoder error";
                    break;
                case 51:
                    error = "One or more of the tracks of the magnetic stripe are blank";
                    break;
                case 52:
                    error = "Flash memory error";
                    break;
                case 53:
                    error = "Cannot access the printer";
                    break;
                case 54:
                    error = "Reception timeout, protocol errors";
                    break;
                case 55:
                    error = "Reception timeout, protocol errors";
                    break;
                case 56:
                    error = "Parameter error";
                    break;
                case 57:
                    error = "Parameter error";
                    break;
                case 60:
                    error = "Printer not supported";
                    break;
                case 61:
                    error = "Unable to open handle to Zebra printer driver";
                    break;
                case 62:
                    error = "Cannot open printer driver";
                    break;
                case 63:
                    error = "One of the arguments is invalid";
                    break;
                case 64:
                    error = "Printer is in use";
                    break;
                case 65:
                    error = "Invalid printer handle";
                    break;
                case 66:
                    error = "Error closing printer driver handle";
                    break;
                case 67:
                    error = "Command failed due to communication error";
                    break;
                case 68:
                    error = "Response too large for buffer";
                    break;
                case 69:
                    error = "Error reading data";
                    break;
                case 70:
                    error = "Error writing data";
                    break;
                case 71:
                    error = "Error loading SDK";
                    break;
                case 72:
                    error = "Invalid structure alignment";
                    break;
                case 73:
                    error = "Unable to create the device context for the driver";
                    break;
                case 74:
                    error = "Print spooler error";
                    break;
                case 75:
                    error = "Operating system is out of memory";
                    break;
                case 76:
                    error = "Operating system is out of disk space";
                    break;
                case 77:
                    error = "Print job aborted by the user";
                    break;
                case 78:
                    error = "Application aborted";
                    break;
                case 79:
                    error = "Error creating file";
                    break;
                case 80:
                    error = "Error writing file";
                    break;
                case 81:
                    error = "Error reading file";
                    break;
                case 82:
                    error = "Invalid media";
                    break;
                case 83:
                    error = "Insufficient system resources to perform necessary memory allocation";
                    break;
                case 255:
                    error = "An unknown but serious exception has occurred";
                    break;
                default:
                    error = "Undefined error";
                    break;
            }
            return error;
        }
    }
}
