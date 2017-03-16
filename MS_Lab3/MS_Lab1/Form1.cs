using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace MS_Lab1
{
    public partial class Form1 : Form
    {
        //int a = Int32.Parse(textBox2.Text);
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
         
        }
        // розміри вікна 
        double ScreenW, ScreenH;
        
        
        int a ;
        int functionRotationAngle = 0;
        int functionRotationLines=0;
        int FunctionRotationSpeed = 1;
        int FunctionRotationLines = 1;
        Color color1;
        Color color2;

        private float devX;
        private float devY;

        // зберігаєм x y точки графіку
        private float[,] GrapValuesArray;
        // кількість елементів  в масиві
        private int elements_count = 0;

        // прапорець, що показує заповненість масиву
        private bool not_calculate = true;

        private void Form1_Load(object sender, EventArgs e)
        {
            a = Int32.Parse(textBox2.Text);
            // виклик бібліотеки
            Glut.glutInit();
            // ініціалізація режиму екрана
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE);

            // встановлення кольору очистки екрана 
            Gl.glClearColor(255, 255, 255, 1);

            // встановлення порту виводу
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            // активація проекційної матриціі
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            // очистка матриці 
            Gl.glLoadIdentity();

            // визначення параметрів налаштувань  проекції в залежності від розмірів сторін елемента AnT. 
            if ((float)AnT.Width <= (float)AnT.Height)
            {
                ScreenW = 30.0;
                ScreenH = 30.0 * (float)AnT.Height / (float)AnT.Width;
                Glu.gluOrtho2D(0.0, ScreenW, 0.0, ScreenH);
            }
            else
            {
                ScreenW = 30.0 * (float)AnT.Width / (float)AnT.Height;
                ScreenH = 30.0;
                Glu.gluOrtho2D(0.0, 15.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0);
            }

            // збереження коефіціентів, які нам необхідні для переведення вказівника в віконній системі в координати, 
            // прийняті в OpenGL сцені 
           devX = (float)ScreenW / (float)AnT.Width;
           devY = (float)ScreenH / (float)AnT.Height;
          
            // установка обєктно-видової матриці
            Gl.glMatrixMode(Gl.GL_MODELVIEW);

           // timer1.Start();

        }

       
     

        private void PrintText2D(float x, float y, string text)
        {

            // встановлюємо позицію виводу 
            Gl.glRasterPos2f(x, y);

           //перебираєм значення text
            foreach (char char_for_draw in text)
            {
                // символ C візуалізуємо з допомогою функції glutBitmapCharacter, викор шрифт GLUT_BITMAP_9_BY_15. 
                Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_9_BY_15, char_for_draw);
            }

        }

          
        private void functionCalculation()
        {

            // визначення локальних змінних X и Y 
            float x = 0, y = 0;

            // масив з якого будуватиметься графік

            GrapValuesArray = new float[600, 2];

            // лічильник елементів  масиву
            elements_count = 0;

            // вирахування всіх y для x, що належать проміжку від  -15 до 15 з кроком 0.01f 
            for (x = -15; x < 15; x += 0.1f)
            {
                // вирахунок х для у
                // по формулі y = (float)Math.Sin(x)*10; 
                // формула. 
                y = a*x*x;

                // запис коорд x 
                GrapValuesArray[elements_count, 0] = x;
                // запис коорд y 
                GrapValuesArray[elements_count, 1] = y;
                // підрахунок елементів 
                elements_count++;

            }
            for (x = -15; x < 15; x += 0.1f)
            {
                y = x;

                GrapValuesArray[elements_count, 0] = 0;
                // запис коорд y 
                GrapValuesArray[elements_count, 1] = y;
                elements_count++;
            }
                // прапорець вирахування координат 
                not_calculate = false;

        }

       

       

        

        private void Draw()
        {

          
            //очистка буфера кольору і буфера глибини
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

          
            //очищення текстури маьтриці
            Gl.glLoadIdentity();

         //установка чорного кольору
            Gl.glColor3f(0, 0, 0);

            //поміщаємо стан матриці в стек матриці
            Gl.glPushMatrix();

            // виконуємо переміщення по х і у
            Gl.glTranslated(15, 15, 0);

            // активація реживу рисування(Вказані точки будуть виводитись як точки GL_POINTS) 
            Gl.glBegin(Gl.GL_POINTS);

         //створюєм сітку із точок
            for (int ax = -15; ax < 15; ax++)
            {
                for (int bx = -15; bx < 15; bx++)
                {
                   
                   
                    Gl.glVertex2d(ax, bx);
                 
                }
            }

          

            // завершуємо малювати примітиви
            Gl.glEnd();
            //Gl.glPushMatrix();
            Gl.glRotated(functionRotationLines, 0, 0, 1);
            // режим малювання, обєднуєм в лінію кожні 2 точки
           
            Gl.glBegin(Gl.GL_LINES);

            // коорд осі
            Gl.glVertex2d(0, -15);
            Gl.glVertex2d(0, 15);

            Gl.glVertex2d(-15, 0);
            Gl.glVertex2d(15, 0);

            // вертикальна стрілка 
            Gl.glVertex2d(0, 15);
            Gl.glVertex2d(0.1, 14.5);
            Gl.glVertex2d(0, 15);
            Gl.glVertex2d(-0.1, 14.5);

            // горизонтальна стрілка 
            Gl.glVertex2d(15, 0);
            Gl.glVertex2d(14.5, 0.1);
            Gl.glVertex2d(15, 0);
            Gl.glVertex2d(14.5, -0.1);

            // завершуємом режим малювання
            Gl.glEnd();
            //Gl.glPopMatrix();
            // виводимо написи на графіках 
            PrintText2D(15.5f, 0, "x");
            PrintText2D(0.5f, 14.5f, "y");

            // викликамо функцію побудови графіка
            Gl.glLineWidth(1);
         

            //Х
            for (int ax = -24; ax <= 20; ax += 2)
            {
                Gl.glBegin(Gl.GL_LINES);
                Gl.glColor3f(0, 0, 0);
                Gl.glVertex2f(-0.5f, ax);
                Gl.glVertex2f(0.5f, ax);

                Gl.glEnd();
                PrintText2D(ax, -1.5f, ax.ToString());
            }

            //У
            for (int ax = 20; ax >= -24; ax -= 2)
            {
                Gl.glBegin(Gl.GL_LINES);
                Gl.glColor3f(0, 0, 0);
                Gl.glVertex2f(ax, -0.5f);
                Gl.glVertex2f(ax, 0.5f);
                Gl.glEnd();
                PrintText2D(-1.5f, ax, ax.ToString());
            }

                // виводимо написи на графіках 
           // PrintText2D(-6.7f, 1.0f, "-PI");
            //PrintText2D(11.5f, 1.0f, "2PI");

        

            // вертаєм матрицю з стека
            Gl.glPopMatrix();

            // очікуємо завершення візуалізації кадра
            Gl.glFlush();

            // сигнал для оновлення елемента 
            AnT.Invalidate();

        }

       

        public void DrawNet()
        {


            Gl.glLineWidth(1);
           

            //Х
            for (int ax = -16; ax <= 14; ax+=2)
            {
                Gl.glBegin(Gl.GL_LINES);
                Gl.glColor3f(0, 0, 0);
                Gl.glVertex2f(-0.5f, ax);
                Gl.glVertex2f(0.5f, ax);

                Gl.glEnd();
                PrintText2D(ax, -1.5f, ax.ToString());
            }

            //У
            for (int ax = 16; ax >= -14; ax-=2)
            {
                Gl.glBegin(Gl.GL_LINES);
                Gl.glColor3f(0, 0, 0);
                Gl.glVertex2f(ax, -0.5f);
                Gl.glVertex2f(ax, 0.5f);
                Gl.glEnd();
                PrintText2D(-1.5f, ax, ax.ToString());
            }
           
            // завершуємо малювати примітиви


            // режим малювання, обєднуєм в лінію кожні 2 точки
       
         
           
            // завершуємом режим малювання
           

            // виводимо написи на графіках 
           // PrintText2D(-6.7f, 1.0f, "-PI");
            //PrintText2D(11.5f, 1.0f, "2PI");

        
         

            // очікуємо завершення візуалізації кадра
            Gl.glFlush();

            // сигнал для оновлення елемента 
            AnT.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            functionRotationAngle+=FunctionRotationSpeed;
            //functionRotationLines += FunctionRotationLines;
            functionCalculation();

            Draw();
            DrawDiagram();
            

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            FunctionRotationSpeed = trackBar1.Value;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            FunctionRotationLines= trackBar2.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button3.BackColor = colorDialog1.Color;
            color1 = colorDialog1.Color;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            button4.BackColor = colorDialog1.Color;
            color2 = colorDialog1.Color;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DrawDiagram()
        {

            // перевіряємо чи координати вже вирахувані
            if (not_calculate)
            {//якщо ні..рахуємо 
                functionCalculation();
            }

            // візуалізація точок -режим
            // точки в лінії(GL_LINE_STRIP) 
             Gl.glLineWidth(5);


            Gl.glPushMatrix();
            Gl.glTranslated(15,15,0);
            Gl.glRotated(functionRotationAngle, 0, 0, 1);
            Gl.glBegin(Gl.GL_LINE_STRIP);

            // рисуємо початкову точку
            Gl.glVertex2d(GrapValuesArray[0, 0], GrapValuesArray[0, 1]);


            // проходимо по масиву точок

            for (int ax = 1; ax < elements_count-300; ax += 2)
            {


                if (GrapValuesArray[ax, 1] < 0) Gl.glColor3f(color1.R/255.0f, color1.G/ax, color1.B/127);
                else
                 if (GrapValuesArray[ax, 1] > 0) Gl.glColor3f(color1.R/ax, color1.G / 255.0f, color1.B/127);

                Gl.glVertex2d(GrapValuesArray[ax, 0], GrapValuesArray[ax, 1]);


            }
            // завершуємо режим малювання
            Gl.glEnd();
            Gl.glPopMatrix();
            Gl.glLineWidth(5);


            Gl.glPushMatrix();
            Gl.glTranslated(15, 15, 0);
            Gl.glRotated(functionRotationAngle, 0, 0, 1);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            for (int ax = 300; ax < elements_count; ax += 2)
            {


                if (GrapValuesArray[ax, 1] < 0) Gl.glColor3f(color1.R / 255.0f, color1.G / ax, color1.B / 127);
                else
                 if (GrapValuesArray[ax, 1] > 0) Gl.glColor3f(color1.R / ax, color1.G / 255.0f, color1.B / 127);

                Gl.glVertex2d(GrapValuesArray[ax, 0], GrapValuesArray[ax, 1]);


            }
            Gl.glEnd();
            Gl.glPopMatrix();
        }

    }
}
