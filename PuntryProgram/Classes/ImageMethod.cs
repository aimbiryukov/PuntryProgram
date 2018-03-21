using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace PuntryProgram.Classes
{
    public static class ImageMethod
    {
        public static Image GetDirectoryImage()
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Файлы изображений (*.gif; *.jpg; *.png; *.bmp)|*.gif;*.jpg;*.png;*.bmp";
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                        return Image.FromFile(openFileDialog.FileName);
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static Binary ImageToBinary(Image image)
        {
            try
            {
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    var resizeImage = ResizeImage(image);
                    resizeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static Image BinaryToImage(Binary binary)
        {
            try
            {
                using (var memoryStream = new System.IO.MemoryStream(binary.ToArray()))
                {
                    return Image.FromStream(memoryStream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static Image ResizeImage(Image image)
        {
            try
            {
                var resizeImage = new Bitmap(1280, 1024);

                using (var graphics = Graphics.FromImage(resizeImage))
                {
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, 0, 0, 1280, 1024);
                    graphics.Dispose();
                    return resizeImage;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
