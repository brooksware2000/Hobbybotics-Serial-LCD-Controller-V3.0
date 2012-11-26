using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using clsSerialPort;

namespace LCD_Demo_V2._2
{
    public partial class Form1 : Form
    {
        clsSerial Comport = new clsSerial();

        private PictureBox[] picArray = new PictureBox[40];

        public Form1()
        {
            InitializeComponent();
            GetPorts();
            InitRowColumn();
            InitializeCgramAddress();
            InitializeCharacterOutputTextBox();
            InitializePictureBoxArray();
        }

        //------------------------------------------------------------------------------
        //  Get list of available serial ports and setup default communications settings
        //------------------------------------------------------------------------------
        private void GetPorts()
        {
            string[] _arrayPorts = null;
            int[] _bitRates = new int[] { 9600, 19200, 38400, 57600 };
            int[] _dataBits = new int[] { 4, 5, 6, 7, 8 };
            string[] _parity = new string[] { "Even", "Odd", "None", "Mark", "Space" };
            int[] _stopBits = new int[] { 1, 2 };
            string[] _flowControl = new string[] { "Xon/Xoff", "Hardware", "None" };

            if (!(Comport == null))
            {
                try
                {
                    _arrayPorts = SerialPort.GetPortNames();
                    Array.Sort(_arrayPorts);

                    cboSelectPort.DataSource = _arrayPorts;
                    cboSelectPort.SelectedIndex = 0;

                    cboSelectBaud.DataSource = _bitRates;
                    cboSelectBaud.SelectedItem = 9600;

                    cboSelectDatBits.DataSource = _dataBits;
                    cboSelectDatBits.SelectedItem = 8;

                    cboSelectParity.DataSource = _parity;
                    cboSelectParity.SelectedItem = "None";

                    cboSelectStopBits.DataSource = _stopBits;
                    cboSelectStopBits.SelectedItem = 1;

                    cboSelectFlowControl.DataSource = _flowControl;
                    cboSelectFlowControl.SelectedItem = "None";
                }

                catch
                {
                    MessageBox.Show("Error opening serial port!", "Message",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        //------------------------------------------------------------------------------
        //  Initialize row and column combo boxes
        //------------------------------------------------------------------------------
        private void InitRowColumn()
        {
            int[] _row = new int[] { 0, 1, 2, 3 };
            int[] _column = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                                        11, 12, 13, 14, 15, 16, 17, 18, 19 };

            cboRow.DataSource = _row;
            cboRow.SelectedIndex = 0;

            cboColumn.DataSource = _column;
            cboColumn.SelectedIndex = 0;
        }

        //------------------------------------------------------------------------------
        //  Initialize CGRAM address combo box
        //------------------------------------------------------------------------------
        private void InitializeCgramAddress()
        {
            int[] address = new int[8];

            for (int i = 0; i < 8; i++)
            {
                address[i] = i;
                cboAddress.Items.Add(address[i]);
            }

            cboAddress.DataSource = address;
            cboAddress.SelectedItem = 0;
        }

        //------------------------------------------------------------------------------
        //  Initialize custom character output textBox
        //------------------------------------------------------------------------------
        private void InitializeCharacterOutputTextBox()
        {
            textBox8.Text = lblChar0.Text + ", " + lblChar1.Text + ", " +
                            lblChar2.Text + ", " + lblChar3.Text + ", " +
                            lblChar4.Text + ", " + lblChar5.Text + ", " +
                            lblChar6.Text + ", " + lblChar7.Text;
        }
        
        //------------------------------------------------------------------------------
        //  Initialize PictureBox control array
        //------------------------------------------------------------------------------
        private void InitializePictureBoxArray()
        {
            picArray[0] = pic0;
            picArray[1] = pic1;
            picArray[2] = pic2;
            picArray[3] = pic3;
            picArray[4] = pic4;
            picArray[5] = pic5;
            picArray[6] = pic6;
            picArray[7] = pic7;
            picArray[8] = pic8;
            picArray[9] = pic9;
            picArray[10] = pic10;
            picArray[11] = pic11;
            picArray[12] = pic12;
            picArray[13] = pic13;
            picArray[14] = pic14;
            picArray[15] = pic15;
            picArray[16] = pic16;
            picArray[17] = pic17;
            picArray[18] = pic18;
            picArray[19] = pic19;
            picArray[20] = pic20;
            picArray[21] = pic21;
            picArray[22] = pic22;
            picArray[23] = pic23;
            picArray[24] = pic24;
            picArray[25] = pic25;
            picArray[26] = pic26;
            picArray[27] = pic27;
            picArray[28] = pic28;
            picArray[29] = pic29;
            picArray[30] = pic30;
            picArray[31] = pic31;
            picArray[32] = pic32;
            picArray[33] = pic33;
            picArray[34] = pic34;
            picArray[35] = pic35;
            picArray[36] = pic36;
            picArray[37] = pic37;
            picArray[38] = pic38;
            picArray[39] = pic39;

            for (int i = 0; i < picArray.Length; ++i)
            {
                picArray[i].Click += new System.EventHandler(this.ClickMe);
                picArray[i].Tag = i;
            }
        }

        //------------------------------------------------------------------------------
        //  Handles click event for PictureBox control array
        //------------------------------------------------------------------------------
        private void ClickMe(object sender, System.EventArgs e)
        {
            PictureBox who = (PictureBox)sender;

            int i = (int)picArray[Convert.ToInt32(who.Tag)].Tag;

            if (picArray[Convert.ToInt32(who.Tag)].BackColor == Color.Black)
            {
                picArray[Convert.ToInt32(who.Tag)].BackColor = Color.White;
                SubBlocks(i);
                CreateCustomCharacter();
            }
            else
            {
                picArray[Convert.ToInt32(who.Tag)].BackColor = Color.Black;
                AddBlocks(i);
                CreateCustomCharacter();
            }
        }

        //------------------------------------------------------------------------------
        //  Updates custom character textBox value
        //------------------------------------------------------------------------------
        private void CreateCustomCharacter()
        {
            textBox8.Text = lblChar0.Text + ", " + lblChar1.Text + ", " +
                            lblChar2.Text + ", " + lblChar3.Text + ", " +
                            lblChar4.Text + ", " + lblChar5.Text + ", " +
                            lblChar6.Text + ", " + lblChar7.Text;
        }

        //------------------------------------------------------------------------------
        //  Add row values and display character value using 5x7 matrix
        //------------------------------------------------------------------------------
        private void AddBlocks(int i)
        {
            const int num4 = 16;
            const int num3 = 8;
            const int num2 = 4;
            const int num1 = 2;
            const int num0 = 1;

            int num = 0;

            switch (i)
            {
                //Row 0
                case 0: num += Convert.ToInt32(lblChar0.Text) + num4;
                    lblChar0.Text = num.ToString();
                    break;
                case 1: num += Convert.ToInt32(lblChar0.Text) + num3;
                    lblChar0.Text = num.ToString();
                    break;
                case 2: num += Convert.ToInt32(lblChar0.Text) + num2;
                    lblChar0.Text = num.ToString();
                    break;
                case 3: num += Convert.ToInt32(lblChar0.Text) + num1;
                    lblChar0.Text = num.ToString();
                    break;
                case 4: num += Convert.ToInt32(lblChar0.Text) + num0;
                    lblChar0.Text = num.ToString();
                    break;

                //Row 1
                case 5: num += Convert.ToInt32(lblChar1.Text) + num4;
                    lblChar1.Text = num.ToString();
                    break;
                case 6: num += Convert.ToInt32(lblChar1.Text) + num3;
                    lblChar1.Text = num.ToString();
                    break;
                case 7: num += Convert.ToInt32(lblChar1.Text) + num2;
                    lblChar1.Text = num.ToString();
                    break;
                case 8: num += Convert.ToInt32(lblChar1.Text) + num1;
                    lblChar1.Text = num.ToString();
                    break;
                case 9: num += Convert.ToInt32(lblChar1.Text) + num0;
                    lblChar1.Text = num.ToString();
                    break;

                //Row 2
                case 10: num += Convert.ToInt32(lblChar2.Text) + num4;
                    lblChar2.Text = num.ToString();
                    break;
                case 11: num += Convert.ToInt32(lblChar2.Text) + num3;
                    lblChar2.Text = num.ToString();
                    break;
                case 12: num += Convert.ToInt32(lblChar2.Text) + num2;
                    lblChar2.Text = num.ToString();
                    break;
                case 13: num += Convert.ToInt32(lblChar2.Text) + num1;
                    lblChar2.Text = num.ToString();
                    break;
                case 14: num += Convert.ToInt32(lblChar2.Text) + num0;
                    lblChar2.Text = num.ToString();
                    break;

                //Row 3
                case 15: num += Convert.ToInt32(lblChar3.Text) + num4;
                    lblChar3.Text = num.ToString();
                    break;
                case 16: num += Convert.ToInt32(lblChar3.Text) + num3;
                    lblChar3.Text = num.ToString();
                    break;
                case 17: num += Convert.ToInt32(lblChar3.Text) + num2;
                    lblChar3.Text = num.ToString();
                    break;
                case 18: num += Convert.ToInt32(lblChar3.Text) + num1;
                    lblChar3.Text = num.ToString();
                    break;
                case 19: num += Convert.ToInt32(lblChar3.Text) + num0;
                    lblChar3.Text = num.ToString();
                    break;

                //Row 4
                case 20: num += Convert.ToInt32(lblChar4.Text) + num4;
                    lblChar4.Text = num.ToString();
                    break;
                case 21: num += Convert.ToInt32(lblChar4.Text) + num3;
                    lblChar4.Text = num.ToString();
                    break;
                case 22: num += Convert.ToInt32(lblChar4.Text) + num2;
                    lblChar4.Text = num.ToString();
                    break;
                case 23: num += Convert.ToInt32(lblChar4.Text) + num1;
                    lblChar4.Text = num.ToString();
                    break;
                case 24: num += Convert.ToInt32(lblChar4.Text) + num0;
                    lblChar4.Text = num.ToString();
                    break;

                //Row 5
                case 25: num += Convert.ToInt32(lblChar5.Text) + num4;
                    lblChar5.Text = num.ToString();
                    break;
                case 26: num += Convert.ToInt32(lblChar5.Text) + num3;
                    lblChar5.Text = num.ToString();
                    break;
                case 27: num += Convert.ToInt32(lblChar5.Text) + num2;
                    lblChar5.Text = num.ToString();
                    break;
                case 28: num += Convert.ToInt32(lblChar5.Text) + num1;
                    lblChar5.Text = num.ToString();
                    break;
                case 29: num += Convert.ToInt32(lblChar5.Text) + num0;
                    lblChar5.Text = num.ToString();
                    break;

                //Row 6
                case 30: num += Convert.ToInt32(lblChar6.Text) + num4;
                    lblChar6.Text = num.ToString();
                    break;
                case 31: num += Convert.ToInt32(lblChar6.Text) + num3;
                    lblChar6.Text = num.ToString();
                    break;
                case 32: num += Convert.ToInt32(lblChar6.Text) + num2;
                    lblChar6.Text = num.ToString();
                    break;
                case 33: num += Convert.ToInt32(lblChar6.Text) + num1;
                    lblChar6.Text = num.ToString();
                    break;
                case 34: num += Convert.ToInt32(lblChar6.Text) + num0;
                    lblChar6.Text = num.ToString();
                    break;

                //Row 7
                case 35: num += Convert.ToInt32(lblChar7.Text) + num4;
                    lblChar7.Text = num.ToString();
                    break;
                case 36: num += Convert.ToInt32(lblChar7.Text) + num3;
                    lblChar7.Text = num.ToString();
                    break;
                case 37: num += Convert.ToInt32(lblChar7.Text) + num2;
                    lblChar7.Text = num.ToString();
                    break;
                case 38: num += Convert.ToInt32(lblChar7.Text) + num1;
                    lblChar7.Text = num.ToString();
                    break;
                case 39: num += Convert.ToInt32(lblChar7.Text) + num0;
                    lblChar7.Text = num.ToString();
                    break;
            }
        }

        //------------------------------------------------------------------------------
        //  Subtract row values and display character value using 5x7 matrix
        //------------------------------------------------------------------------------
        private void SubBlocks(int Tag)
        {
            const int num4 = 16;
            const int num3 = 8;
            const int num2 = 4;
            const int num1 = 2;
            const int num0 = 1;

            int num = 0;

            switch (Tag)
            {
                //Row 0
                case 0: num += Convert.ToInt32(lblChar0.Text) - num4;
                    lblChar0.Text = num.ToString();
                    break;
                case 1: num += Convert.ToInt32(lblChar0.Text) - num3;
                    lblChar0.Text = num.ToString();
                    break;
                case 2: num += Convert.ToInt32(lblChar0.Text) - num2;
                    lblChar0.Text = num.ToString();
                    break;
                case 3: num += Convert.ToInt32(lblChar0.Text) - num1;
                    lblChar0.Text = num.ToString();
                    break;
                case 4: num += Convert.ToInt32(lblChar0.Text) - num0;
                    lblChar0.Text = num.ToString();
                    break;

                //Row 1
                case 5: num += Convert.ToInt32(lblChar1.Text) - num4;
                    lblChar1.Text = num.ToString();
                    break;
                case 6: num += Convert.ToInt32(lblChar1.Text) - num3;
                    lblChar1.Text = num.ToString();
                    break;
                case 7: num += Convert.ToInt32(lblChar1.Text) - num2;
                    lblChar1.Text = num.ToString();
                    break;
                case 8: num += Convert.ToInt32(lblChar1.Text) - num1;
                    lblChar1.Text = num.ToString();
                    break;
                case 9: num += Convert.ToInt32(lblChar1.Text) - num0;
                    lblChar1.Text = num.ToString();
                    break;

                //Row 2
                case 10: num += Convert.ToInt32(lblChar2.Text) - num4;
                    lblChar2.Text = num.ToString();
                    break;
                case 11: num += Convert.ToInt32(lblChar2.Text) - num3;
                    lblChar2.Text = num.ToString();
                    break;
                case 12: num += Convert.ToInt32(lblChar2.Text) - num2;
                    lblChar2.Text = num.ToString();
                    break;
                case 13: num += Convert.ToInt32(lblChar2.Text) - num1;
                    lblChar2.Text = num.ToString();
                    break;
                case 14: num += Convert.ToInt32(lblChar2.Text) - num0;
                    lblChar2.Text = num.ToString();
                    break;

                //Row 3
                case 15: num += Convert.ToInt32(lblChar3.Text) - num4;
                    lblChar3.Text = num.ToString();
                    break;
                case 16: num += Convert.ToInt32(lblChar3.Text) - num3;
                    lblChar3.Text = num.ToString();
                    break;
                case 17: num += Convert.ToInt32(lblChar3.Text) - num2;
                    lblChar3.Text = num.ToString();
                    break;
                case 18: num += Convert.ToInt32(lblChar3.Text) - num1;
                    lblChar3.Text = num.ToString();
                    break;
                case 19: num += Convert.ToInt32(lblChar3.Text) - num0;
                    lblChar3.Text = num.ToString();
                    break;

                //Row 4
                case 20: num += Convert.ToInt32(lblChar4.Text) - num4;
                    lblChar4.Text = num.ToString();
                    break;
                case 21: num += Convert.ToInt32(lblChar4.Text) - num3;
                    lblChar4.Text = num.ToString();
                    break;
                case 22: num += Convert.ToInt32(lblChar4.Text) - num2;
                    lblChar4.Text = num.ToString();
                    break;
                case 23: num += Convert.ToInt32(lblChar4.Text) - num1;
                    lblChar4.Text = num.ToString();
                    break;
                case 24: num += Convert.ToInt32(lblChar4.Text) - num0;
                    lblChar4.Text = num.ToString();
                    break;

                //Row 5
                case 25: num += Convert.ToInt32(lblChar5.Text) - num4;
                    lblChar5.Text = num.ToString();
                    break;
                case 26: num += Convert.ToInt32(lblChar5.Text) - num3;
                    lblChar5.Text = num.ToString();
                    break;
                case 27: num += Convert.ToInt32(lblChar5.Text) - num2;
                    lblChar5.Text = num.ToString();
                    break;
                case 28: num += Convert.ToInt32(lblChar5.Text) - num1;
                    lblChar5.Text = num.ToString();
                    break;
                case 29: num += Convert.ToInt32(lblChar5.Text) - num0;
                    lblChar5.Text = num.ToString();
                    break;

                //Row 6
                case 30: num += Convert.ToInt32(lblChar6.Text) - num4;
                    lblChar6.Text = num.ToString();
                    break;
                case 31: num += Convert.ToInt32(lblChar6.Text) - num3;
                    lblChar6.Text = num.ToString();
                    break;
                case 32: num += Convert.ToInt32(lblChar6.Text) - num2;
                    lblChar6.Text = num.ToString();
                    break;
                case 33: num += Convert.ToInt32(lblChar6.Text) - num1;
                    lblChar6.Text = num.ToString();
                    break;
                case 34: num += Convert.ToInt32(lblChar6.Text) - num0;
                    lblChar6.Text = num.ToString();
                    break;

                //Row 7
                case 35: num += Convert.ToInt32(lblChar7.Text) - num4;
                    lblChar7.Text = num.ToString();
                    break;
                case 36: num += Convert.ToInt32(lblChar7.Text) - num3;
                    lblChar7.Text = num.ToString();
                    break;
                case 37: num += Convert.ToInt32(lblChar7.Text) - num2;
                    lblChar7.Text = num.ToString();
                    break;
                case 38: num += Convert.ToInt32(lblChar7.Text) - num1;
                    lblChar7.Text = num.ToString();
                    break;
                case 39: num += Convert.ToInt32(lblChar7.Text) - num0;
                    lblChar7.Text = num.ToString();
                    break;
            }
        }

        //------------------------------------------------------------------------------
        //  Delay function
        //------------------------------------------------------------------------------        
        public static DateTime PauseForMilliSeconds(int MilliSecondsToPauseFor)
        {


            System.DateTime ThisMoment = System.DateTime.Now;
            System.TimeSpan duration = new System.TimeSpan(0, 0, 0, 0, MilliSecondsToPauseFor);
            System.DateTime AfterWards = ThisMoment.Add(duration);


            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = System.DateTime.Now;
            }


            return System.DateTime.Now;
        }
        
        //------------------------------------------------------------------------------
        //  If serial port is open then, close it prior to exiting application
        //------------------------------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Comport.DisconnectPort();
        }
        
        //------------------------------------------------------------------------------
        //  Connect to the serial port if available
        //------------------------------------------------------------------------------
        private void btnConnectPort_Click(object sender, EventArgs e)
        {
            Comport.ConnectPort(cboSelectPort.SelectedValue.ToString(),
                                cboSelectBaud.SelectedValue.ToString(),
                                cboSelectDatBits.SelectedValue.ToString(),
                                cboSelectParity.SelectedValue.ToString(),
                                cboSelectStopBits.SelectedValue.ToString(),
                                cboSelectFlowControl.SelectedValue.ToString()); 
            
            //Comport.ConnectPort(cboSelectPort.SelectedValue.ToString(), cboSelectBaud.SelectedValue.ToString());
                                
            toolStripStatusLabel1.Text = "Status: " + "Connected to " + 
                                         cboSelectPort.SelectedValue + 
                                         " at " + cboSelectBaud.SelectedValue;

            Comport.ClearScreen();
            PauseForMilliSeconds(100);
            Comport.WriteText("Connected @ " + cboSelectBaud.SelectedValue);
        }

        //------------------------------------------------------------------------------
        //  Disconnect from serial port
        //------------------------------------------------------------------------------
        private void btnDisconnectPort_Click(object sender, EventArgs e)
        {
            Comport.ClearScreen();
            PauseForMilliSeconds(100);
            Comport.WriteText("Disconnected @ " + cboSelectBaud.SelectedValue);
            Comport.DisconnectPort();
            toolStripStatusLabel1.Text = "Status: " + "Disconnected from " +
                                         cboSelectPort.SelectedValue;
        }

        //------------------------------------------------------------------------------
        //  Change the baud rate
        //------------------------------------------------------------------------------
        private void btnChangeBaud_Click(object sender, EventArgs e)
        {
            switch (cboSelectBaud.SelectedIndex)
            {
                case 0: Comport.SetBaudRate(0); break;  //9600
                case 1: Comport.SetBaudRate(1); break;  //19200
                case 2: Comport.SetBaudRate(2); break;  //38400
                case 3: Comport.SetBaudRate(3); break;  //57600
                default: Comport.SetBaudRate(0); break; //9600
            }
            
            toolStripStatusLabel1.Text = "Status: " + "Connected to " +
                                         cboSelectPort.SelectedValue +
                                         " at " + cboSelectBaud.SelectedValue;

            Comport.DisconnectPort();
            //Comport.ConnectPort(cboSelectPort.SelectedValue.ToString(), cboSelectBaud.SelectedValue.ToString());
            Comport.ConnectPort(cboSelectPort.SelectedValue.ToString(),
                                cboSelectBaud.SelectedValue.ToString(),
                                cboSelectDatBits.SelectedValue.ToString(),
                                cboSelectParity.SelectedValue.ToString(),
                                cboSelectStopBits.SelectedValue.ToString(),
                                cboSelectFlowControl.SelectedValue.ToString());

            Comport.ClearScreen();
            PauseForMilliSeconds(100);
            Comport.WriteText("Connected @ " + cboSelectBaud.SelectedValue);
        }

        //------------------------------------------------------------------------------
        //  Close port and exit application
        //------------------------------------------------------------------------------
        private void btnExit_Click(object sender, EventArgs e)
        {
            Comport.DisconnectPort();
            Application.Exit();
        }

        //------------------------------------------------------------------------------
        //  Write text to line 1
        //------------------------------------------------------------------------------
        private void btnSendText1_Click(object sender, EventArgs e)
        {
            Comport.SetCursorPosition(0);
            Comport.WriteText(textBox1.Text);
        }

        //------------------------------------------------------------------------------
        //  Write text to line 2
        //------------------------------------------------------------------------------
        private void btnSendText2_Click(object sender, EventArgs e)
        {
            Comport.SetCursorPosition(64);
            Comport.WriteText(textBox2.Text);
        }

        //------------------------------------------------------------------------------
        //  Write text to line 3
        //------------------------------------------------------------------------------
        private void btnSendText3_Click(object sender, EventArgs e)
        {
            Comport.SetCursorPosition(20);
            Comport.WriteText(textBox3.Text);
        }

        //------------------------------------------------------------------------------
        //  Write text to line 4
        //------------------------------------------------------------------------------
        private void btnSendText4_Click(object sender, EventArgs e)
        {
            Comport.SetCursorPosition(84);
            Comport.WriteText(textBox4.Text);
        }

        //------------------------------------------------------------------------------
        //  Write text to all lines
        //------------------------------------------------------------------------------
        private void btnSendAll_Click(object sender, EventArgs e)
        {
            Comport.SetCursorPosition(0);
            Comport.WriteText(textBox1.Text);
            Comport.SetCursorPosition(64);
            Comport.WriteText(textBox2.Text);
            Comport.SetCursorPosition(20);
            Comport.WriteText(textBox3.Text);
            Comport.SetCursorPosition(84);
            Comport.WriteText(textBox4.Text);
        }

        //------------------------------------------------------------------------------
        //  Clear text from line 1
        //------------------------------------------------------------------------------
        private void btnClearText1_Click(object sender, EventArgs e)
        {
            Comport.ClearLine(1);
        }

        //------------------------------------------------------------------------------
        //  Clear text from line 2
        //------------------------------------------------------------------------------
        private void btnClearText2_Click(object sender, EventArgs e)
        {
            Comport.ClearLine(2);
        }

        //------------------------------------------------------------------------------
        //  Clear text from line 4
        //------------------------------------------------------------------------------
        private void btnClearText3_Click(object sender, EventArgs e)
        {
            Comport.ClearLine(3);
        }

        //------------------------------------------------------------------------------
        //  Clear text from line 4
        //------------------------------------------------------------------------------
        private void btnClearText4_Click(object sender, EventArgs e)
        {
            Comport.ClearLine(4);
        }

        //------------------------------------------------------------------------------
        //  Clear all text
        //------------------------------------------------------------------------------
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            Comport.ClearScreen();
        }

        //------------------------------------------------------------------------------
        //  Adjust display brightness level
        //------------------------------------------------------------------------------
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            lblLevel.Text = "Level = " + trackBar1.Value.ToString();
            if (trackBar1.Value < 1) trackBar1.Value = 1;
            Comport.SetBacklightLevel(trackBar1.Value);
            Comport.SetCursorPosition(64);
            Comport.WriteText("Level = " + trackBar1.Value.ToString() + " ");
        }

        //------------------------------------------------------------------------------
        //  Turn on backlight and restore brightness
        //------------------------------------------------------------------------------
        private void btnBacklightOn_Click(object sender, EventArgs e)
        {
            Comport.SetBacklightState(1);
            Comport.SetBacklightLevel(trackBar1.Value);
        }

        //------------------------------------------------------------------------------
        //  Turn off backlight
        //------------------------------------------------------------------------------
        private void btnBacklightOff_Click(object sender, EventArgs e)
        {
            Comport.SetBacklightLevel(0);
            Comport.SetBacklightState(0);
        }

        //------------------------------------------------------------------------------
        //  Stores current backlight level
        //------------------------------------------------------------------------------
        private void btnStoreLevel_Click(object sender, EventArgs e)
        {
            Comport.SaveBacklightLevel(trackBar1.Value);
            Comport.ClearScreen();
            PauseForMilliSeconds(100);
            Comport.WriteText("Level Saved");
        }

        //------------------------------------------------------------------------------
        //  Recalls saved backlight level
        //------------------------------------------------------------------------------
        private void btnRecallLevel_Click(object sender, EventArgs e)
        {
            Comport.RecallBacklightLevel();
            Comport.ClearScreen();
            PauseForMilliSeconds(100);
            Comport.WriteText("Level Recalled");
        }

        //------------------------------------------------------------------------------
        //  Enables display
        //------------------------------------------------------------------------------
        private void btnEnableDisplay_Click(object sender, EventArgs e)
        {
            Comport.SetDisplayProperties(1);
        }

        //------------------------------------------------------------------------------
        //  Disables display
        //------------------------------------------------------------------------------
        private void btnDisableDisplay_Click(object sender, EventArgs e)
        {
            Comport.SetDisplayProperties(0);
        }

        //------------------------------------------------------------------------------
        //  Turns on block cursor
        //------------------------------------------------------------------------------
        private void btnBlockCursor_Click(object sender, EventArgs e)
        {
            Comport.SetDisplayProperties(2);
        }

        //------------------------------------------------------------------------------
        //  Turns on underscore cursor
        //------------------------------------------------------------------------------
        private void btnUnderscoreCursor_Click(object sender, EventArgs e)
        {
            Comport.SetDisplayProperties(3);
        }

        //------------------------------------------------------------------------------
        //  Turns on both cursors
        //------------------------------------------------------------------------------
        private void btnBothCursors_Click(object sender, EventArgs e)
        {
            Comport.SetDisplayProperties(4);
        }

        //------------------------------------------------------------------------------
        //  Shift cursor left
        //------------------------------------------------------------------------------        
        private void btnShiftCursorLeft_Click(object sender, EventArgs e)
        {
            Comport.ShiftCursor(0);
        }

        //------------------------------------------------------------------------------
        //  Shift cursor right
        //------------------------------------------------------------------------------
        private void btnShiftCursorRight_Click(object sender, EventArgs e)
        {
            Comport.ShiftCursor(1);
        }

        //------------------------------------------------------------------------------
        //  Shift display left
        //------------------------------------------------------------------------------
        private void btnShiftDisplayLeft_Click(object sender, EventArgs e)
        {
            Comport.ShiftDisplay(0);
        }

        //------------------------------------------------------------------------------
        //  Shift display right
        //------------------------------------------------------------------------------
        private void btnShiftDisplayRight_Click(object sender, EventArgs e)
        {
            Comport.ShiftDisplay(1);
        }

        //------------------------------------------------------------------------------
        //  Backspace and delete character
        //------------------------------------------------------------------------------
        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            Comport.Backspace();
        }

        //------------------------------------------------------------------------------
        //  Move cursor to upper left-most position
        //------------------------------------------------------------------------------
        private void btnHome_Click(object sender, EventArgs e)
        {
            Comport.Home();
        }

        //------------------------------------------------------------------------------
        //  Move cursor to first position of next line
        //------------------------------------------------------------------------------
        private void btnReturn_Click(object sender, EventArgs e)
        {
            Comport.CarriageReturn();
        }

        //------------------------------------------------------------------------------
        //  Scroll cursor up
        //------------------------------------------------------------------------------
        private void btnScrollUp_Click(object sender, EventArgs e)
        {
            Comport.ScrollUp();
        }

        //------------------------------------------------------------------------------
        //  Scroll cursor down
        //------------------------------------------------------------------------------
        private void btnScrollDown_Click(object sender, EventArgs e)
        {
            Comport.ScrollDown();
        }

        //------------------------------------------------------------------------------
        //  Sets cursor position on display
        //------------------------------------------------------------------------------
        private void btnSetCursorPosition_Click(object sender, EventArgs e)
        {
            int row = 0;
            int column = 0;
            
            switch (Convert.ToInt32(cboRow.SelectedValue))
            {
                case 0: row = 0; break;
                case 1: row = 64; break;
                case 2: row = 20; break;
                case 3: row = 84; break;
            }

            column = row + Convert.ToInt32(cboColumn.SelectedValue);
            Comport.SetCursorPosition(column);
        }

        //------------------------------------------------------------------------------
        //  Send ASCII characters to display
        //------------------------------------------------------------------------------
        private void btnSendAscii_Click(object sender, EventArgs e)
        {
            try
            {
                Comport.WriteASCII(textBox7.Text);
            }

            catch
            {
                MessageBox.Show("Only Decimal representation of ASCII characters allowed", 
                                "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //------------------------------------------------------------------------------
        //  Fill all custom character boxes
        //------------------------------------------------------------------------------
        private void btnFillBlocks_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < picArray.Length; ++i)
            {
                picArray[i].BackColor = Color.Black;
            }

            lblChar0.Text = "31";
            lblChar1.Text = "31";
            lblChar2.Text = "31";
            lblChar3.Text = "31";
            lblChar4.Text = "31";
            lblChar5.Text = "31";
            lblChar6.Text = "31";
            lblChar7.Text = "31";

            CreateCustomCharacter();
        }

        //------------------------------------------------------------------------------
        //  Clear all custom character boxes
        //------------------------------------------------------------------------------
        private void btnClearBlocks_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < picArray.Length; ++i)
            {
                picArray[i].BackColor = Color.White;
            }

            lblChar0.Text = "0";
            lblChar1.Text = "0";
            lblChar2.Text = "0";
            lblChar3.Text = "0";
            lblChar4.Text = "0";
            lblChar5.Text = "0";
            lblChar6.Text = "0";
            lblChar7.Text = "0";

            CreateCustomCharacter();
        }

        //------------------------------------------------------------------------------
        //  Invert color of all custom character boxes to opposite of current color
        //------------------------------------------------------------------------------
        private void btnInvertBlocks_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < picArray.Length; ++i)
            {
                if (picArray[i].BackColor == Color.Black)
                {
                    picArray[i].BackColor = Color.White;
                    SubBlocks(i);
                }

                else
                {
                    picArray[i].BackColor = Color.Black;
                    AddBlocks(i);
                }
            }
            CreateCustomCharacter();
        }

        //------------------------------------------------------------------------------
        //  Save custom character to CGRAM address location
        //------------------------------------------------------------------------------
        private void btnStoreCharacter_Click(object sender, EventArgs e)
        {
            int cgram = 0;
            int[] charS = new int[8];
            string text = null;

            text = textBox8.Text;

            string[] characters = text.Split(',');

            charS[0] = Convert.ToInt32(characters[0]);
            charS[1] = Convert.ToInt32(characters[1]);
            charS[2] = Convert.ToInt32(characters[2]);
            charS[3] = Convert.ToInt32(characters[3]);
            charS[4] = Convert.ToInt32(characters[4]);
            charS[5] = Convert.ToInt32(characters[5]);
            charS[6] = Convert.ToInt32(characters[6]);
            charS[7] = Convert.ToInt32(characters[7]);

            switch (Convert.ToInt32(cboAddress.SelectedValue))
            {
                case 0: cgram = 0; break;
                case 1: cgram = 8; break;
                case 2: cgram = 16; break;
                case 3: cgram = 24; break;
                case 4: cgram = 32; break;
                case 5: cgram = 40; break;
                case 6: cgram = 48; break;
                case 7: cgram = 56; break;
            }
            Comport.WriteCustomCharacter(cgram, charS[0], charS[1], charS[2], charS[3], charS[4], charS[5], charS[6], charS[7]);
        }

        //------------------------------------------------------------------------------
        //  Display selected custom character
        //------------------------------------------------------------------------------
        private void btnDisplayCharacter_Click(object sender, EventArgs e)
        {
            int cgram = Convert.ToInt32(cboAddress.SelectedValue);
            Comport.DisplayCustomCharacter(cgram);
        }

        //------------------------------------------------------------------------------
        //  Enables splash screen on start up
        //------------------------------------------------------------------------------
        private void btnEnableSplashScreen_Click(object sender, EventArgs e)
        {
            Comport.SetSplashScreen(1);
        }

        //------------------------------------------------------------------------------
        //  Disables splash screen on start up
        //------------------------------------------------------------------------------
        private void btnDisableSplashScreen_Click(object sender, EventArgs e)
        {
            Comport.SetSplashScreen(0);
        }
    }
}
