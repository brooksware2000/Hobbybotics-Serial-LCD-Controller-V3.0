using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms;

namespace clsSerialPort
{
    public class clsSerial
    {
        private SerialPort Comport = new SerialPort();

        //Name of communication port
        private string _portName = string.Empty;
        //speed of communications equivalent to bits per second
        private string _baudRate = string.Empty;
        //Number of bits representing one character of data
        private string _dataBits = string.Empty;
        //Bit that signals end of a transmission unit
        private string _stopBits = string.Empty;
        //Used to determine the integrity of data after a transmission
        private string _parity = string.Empty;
        //Controls the rate of flow of data transmission
        private string _flowControl = string.Empty;

        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }
        
        public string BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        public string DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        public string StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        public string Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        //------------------------------------------------------------------------------
        //  Connect to the serial port if available
        //------------------------------------------------------------------------------
        public void ConnectPort(string portname, string baudrate, 
                                string databits, string parity,
                                string stopbits, string flowcontrol)
        {
            _portName = portname;
            _baudRate = baudrate;
            _dataBits = databits;
            _parity = parity;
            _stopBits = stopbits;
            _flowControl = flowcontrol;

            try
            {
                if (Comport.IsOpen == true) Comport.Close();

                Comport.BaudRate = Convert.ToInt32(_baudRate);//int.Parse(_baudRate);
                Comport.PortName = _portName;
                Comport.DataBits = Convert.ToInt32(_dataBits);//int.Parse(_dataBits);
                Comport.Parity = (Parity)Enum.Parse(typeof(Parity), _parity);
                Comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits);
                Comport.Handshake = (Handshake)Enum.Parse(typeof(Handshake), _flowControl);
                Comport.Encoding = System.Text.Encoding.Default;
                Comport.ReadTimeout = 1000;
                Comport.Open();
                
            }

            catch(IOException ex)
            {
                //MessageBox.Show("Error opening serial port!", 
                //"Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------------------------------------
        //  Disconnect from serial port
        //------------------------------------------------------------------------------
        public void DisconnectPort()
        {
            try
            {
                if (Comport.IsOpen == true) Comport.Close();
            }

            catch (IOException ex)
            {
                //MessageBox.Show("Error opening serial port!", 
                //"Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------------------------------------
        //  Write text to display
        //------------------------------------------------------------------------------
        public void WriteText(string text)
        {
            if (Comport.IsOpen) Comport.Write(text);
        }

        //------------------------------------------------------------------------------
        //  Send ASCII characters to display
        //------------------------------------------------------------------------------
        public void WriteASCII(string ASCII)
        {
            //ASCII = ASCII.Replace(" ", "");
            //Byte[] dataToSend = new Byte[] { Convert.ToByte(ASCII.Length) };
            //if (Comport.IsOpen) Comport.Write(dataToSend,  0, dataToSend.Length); 

            ASCII = ASCII.Replace(" ", "");

            Byte[] dataToSend = new Byte[ASCII.Length];

            dataToSend[0] = Convert.ToByte(ASCII);

            if (Comport.IsOpen)
            {
                Comport.Write(dataToSend, 0, 1);
            } 
        }

        //------------------------------------------------------------------------------
        //  Clear display contents
        //------------------------------------------------------------------------------
        public void ClearScreen()
        {
            Byte[] dataToSend = new Byte[] { 254, 0 };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Set cursor to upper left-most position
        //------------------------------------------------------------------------------
        public void Home()
        {
            Byte[] dataToSend = new Byte[] { 254, 1 };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Shift cursor left/right
        //------------------------------------------------------------------------------
        //0 = left, 1 = right
        public void ShiftCursor(int direction)
        {
            Byte[] dataToSend = new Byte[] { 254, 2, Convert.ToByte(direction) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Shift display left/right
        //------------------------------------------------------------------------------
        //0 = left, 1 = right
        public void ShiftDisplay(int direction)
        {
            Byte[] dataToSend = new Byte[] { 254, 3, Convert.ToByte(direction) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Set cursor position to row/column location
        //------------------------------------------------------------------------------
        public void SetCursorPosition(int position)
        {
            Byte[] dataToSend = new Byte[] { 254, 4, Convert.ToByte(position) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Set cursor and display properties
        //------------------------------------------------------------------------------
        public void SetDisplayProperties(int mode)
        {
            Byte[] dataToSend = new Byte[] { 254, 5, Convert.ToByte(mode) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Deletes a character
        //------------------------------------------------------------------------------
        public void Backspace()
        {
            Byte[] dataToSend = new Byte[] { 254, 6 };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Send custom characters to display memory
        //------------------------------------------------------------------------------
        public void WriteCustomCharacter(int cgram, int char0, int char1, int char2, 
                                         int char3, int char4, int char5, int char6, 
                                         int char7)
        {
            Byte[] dataToSend = new Byte[] { 254, 7, Convert.ToByte(cgram),
                                             Convert.ToByte(char0), Convert.ToByte(char1),
                                             Convert.ToByte(char2), Convert.ToByte(char3),
                                             Convert.ToByte(char4), Convert.ToByte(char5),
                                             Convert.ToByte(char6), Convert.ToByte(char7)};
    
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Display custom characters stored in memory
        //------------------------------------------------------------------------------
        public void DisplayCustomCharacter(int cgram)
        {
            Byte[] dataToSend = new Byte[] { 254, 8, Convert.ToByte(cgram) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Turn off/on backlight
        //------------------------------------------------------------------------------
        public void SetBacklightState(int state)
        {
            Byte[] dataToSend = new Byte[] { 254, 9, Convert.ToByte(state) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Adjust display brightness level
        //------------------------------------------------------------------------------
        public void SetBacklightLevel(int level)
        {
            Byte[] dataToSend = new Byte[] { 254, 10, Convert.ToByte(level) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Change the baud rate
        //------------------------------------------------------------------------------
        public void SetBaudRate(int baudrate)
        {
            Byte[] dataToSend = new Byte[] { 254, 11, Convert.ToByte(baudrate) };
            if (Comport.IsOpen)
            {
                Comport.Write(dataToSend, 0, dataToSend.Length);
            }
        }

        //------------------------------------------------------------------------------
        //  Stores current backlight level
        //------------------------------------------------------------------------------
        public void SaveBacklightLevel(int level)
        {
            Byte[] dataToSend = new Byte[] { 254, 12, Convert.ToByte(level) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Recalls saved backlight level
        //------------------------------------------------------------------------------
        public void RecallBacklightLevel()
        {
            Byte[] dataToSend = new Byte[] { 254, 13 };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Clears selected line
        //------------------------------------------------------------------------------
        public void ClearLine(int row)
        {
            Byte[] dataToSend = new Byte[] { 254, 14, Convert.ToByte(row) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Puts cursor at first position of next line
        //------------------------------------------------------------------------------
        public void CarriageReturn()
        {
            Byte[] dataToSend = new Byte[] { 254, 15 };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Scrolls cursor up
        //------------------------------------------------------------------------------
        public void ScrollUp()
        {
            Byte[] dataToSend = new Byte[] { 254, 16 };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Scrolls cursor down
        //------------------------------------------------------------------------------
        public void ScrollDown()
        {
            Byte[] dataToSend = new Byte[] { 254, 17 };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }

        //------------------------------------------------------------------------------
        //  Sets default value to enable/disable splash screen
        //------------------------------------------------------------------------------
        public void SetSplashScreen(int value)
        {
            Byte[] dataToSend = new Byte[] { 254, 18, Convert.ToByte(value) };
            if (Comport.IsOpen) Comport.Write(dataToSend, 0, dataToSend.Length);
        }
    }
}
