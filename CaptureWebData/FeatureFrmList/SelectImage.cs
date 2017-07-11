using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeatureFrmList
{
    public partial class SelectHeadImage : UserControl
    {
        
        public SelectHeadImage()
        {
            InitializeComponent();
            Init();
        }
        private void Init() 
        {
            openImageDialog.Title = "选择图片";
            openImageDialog.Filter = "图片(*.jpg)|*.jpg;*.png;*.gif;*.bmp";
        }
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            { 
                //获取要展示的图片文件流

            }
        }

    }
}
