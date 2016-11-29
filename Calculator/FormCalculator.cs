using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class form_Calcutor : Form
    {
        static List<Button> numbers;
        static List<Button> operas;
        static Double x=0, y=0, res=0;
        static Int32 dem_dau = 0;
        static String tempOpera="4";
        public form_Calcutor()
        {
            InitializeComponent();

            //Đặt giá trị cho lable
            lbOpera.Text = "0";
            lbResult.Text = "= 0";

            // Ánh xạ các đối tượng trên form vào List
            numbers = new List<Button> { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };
            operas = new List<Button> { btnDel, btnAdd, btnSub, btnMul, btnDiv, btnResult, btnDot };

            //Đăng ký sự kiện cho các Button
            foreach (Button num in numbers)
            {
                num.Click += new EventHandler(Button_Click);
                num.TabStop = false;
                num.FlatStyle = FlatStyle.Flat;
                num.FlatAppearance.BorderSize = 0;

            }

            foreach (Button ope in operas)
            {
                ope.Click += new EventHandler(Button_Click);
                ope.TabStop = false;
                ope.FlatStyle = FlatStyle.Flat;
                ope.FlatAppearance.BorderSize = 0;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Text;
            funcValidate(buttonText);
        }

        //Bắt lỗi 
        private void funcValidate(string str)
        {
            //TH: Click 'AC' hoặc click vào chỉ ghi nhận "-" đầu tiên
            if (str == "AC")
            {
                lbOpera.Text =  "0";
                lbResult.Text = "= 0";
                tempOpera = "";
                x = y = res = 0;
            }
            //TH: Click vao button ","
            else if (str == ",")
            {
                // Đếm số lượng dấu ','
                int count = lbOpera.Text.Count(f => f == ',');

                //Tính toán biểu thức có dạng x + y = ?. Nên chỉ có thể nhiều nhất 2 dấu ','
                if (tempOpera == "" || count > 1)
                    MessageBox.Show("Không phải là số !");
                else if (lbOpera.Text != "")
                    lbOpera.Text += str;
            }
            // Click button "="
            else if (str == "=")
            {
                string last = lbOpera.Text[lbOpera.Text.Length - 1].ToString();
                // Ký tự cuối cùng là biểu thức hoặc dấu ","
                if (last == ",")
                    MessageBox.Show("Lỗi toán học !");
                else
                {
                    if (lbOpera.Text[0] == '-' && lbOpera.Text.Length > 1)
                        lbResult.Text = "= "+ lbOpera.Text;
                    try
                    {
                        int index_y;

                        // Tìm vị trí của dấu biểu thức
                        if (tempOpera != lbOpera.Text[0].ToString())
                            index_y = lbOpera.Text.IndexOf(tempOpera);
                        else
                            index_y = lbOpera.Text.LastIndexOf(tempOpera);

                        string sY = lbOpera.Text.Substring(index_y + 1);
                        y = Convert.ToDouble(sY);

                        funcTinhToan();
                        dem_dau = 0;
                        tempOpera = "";
                        lbResult.Text = "= " + res.ToString();
                    }
                    catch { }
                    
                }
                    
            }
            // Click vao button dấu của biểu thức
            else if(kt_toantu(str))
            {
                if (lbOpera.Text == "0" && str == "-" )
                {
                    lbOpera.Text = str;
                    // Lưu lại dấu âm
                    tempOpera = "-";
                }
                //TH: Có tồn tại 2 dấu toán học gần nhau. Chỉ ghi nhận dấu được click sau
                else if (kt_toantu(lbOpera.Text[lbOpera.Text.Length - 1].ToString()))
                {
                    lbOpera.Text = lbOpera.Text.Remove(lbOpera.Text.Length - 1);
                    lbOpera.Text += str;
                }
                else
                {
                    if (dem_dau == 0)
                    {
                        if (x != y || x != res)
                        {
                            lbOpera.Text = res.ToString();
                            res = 0;
                        }
                            
                        string sX = lbOpera.Text.Substring(0);
                        x = Convert.ToDouble(sX);
                        //Cập nhật dấu
                        tempOpera = str;
                        //Biểu thức đủ dấu
                        dem_dau++;
                    }
                    else
                    {
                        int index_y = lbOpera.Text.IndexOf(tempOpera);
                        string sY = lbOpera.Text.Substring(index_y + 1);
                        y = Convert.ToDouble(sY);

                        //Hiển thị kết quả
                        funcTinhToan();
                        lbResult.Text =  "= "+res.ToString();
                        lbOpera.Text = res.ToString();
                        //Cập nhật x
                        x = Convert.ToDouble(res);

                        //Cập nhật tempOpera
                        tempOpera = str;
                    }

                    lbOpera.Text += str;
                }
                      
            }
            //Click button number
            else
            {
                //Xử lý không để số 0 ở đầu các số có nhiều hơn 2 chữ số tránh vô nghĩa
                if (lbOpera.Text != "0" && !check_00())
                    lbOpera.Text += str;
                else if(lbOpera.Text[lbOpera.Text.Length - 1].ToString() == "0")
                {
                    lbOpera.Text = lbOpera.Text.Remove(lbOpera.Text.Length - 1);
                    lbOpera.Text += str;
                }
                else 
                    lbOpera.Text = str;
            }
                
        }

        //Tính toán
        private void funcTinhToan()
        {
            switch(tempOpera)
            {
                case "+": res = x + y; break;
                case "-": res = x - y; break;
                case "*": res = x * y; break;
                case "/": res = x / y; break;
            }
        }

        //Kiểm tra xem phải dấu biểu thức không
        private bool kt_toantu(String str)
        {
            if (str == "." || str == "+" || str == "-" || str == "*" || str == "/")
                return true;
            else
                return false;
        }

        //Không để thừa số 0 ở đầu các số nhiều hơn 2 chữ số
        private bool check_00()
        {
            for(int i=lbOpera.Text.Length-1;i>=1;i--)
            {
                if (kt_toantu(lbOpera.Text[i - 1].ToString()) && lbOpera.Text[i] == '0')
                    return true;
            }
            return false;
        }
    }
}
