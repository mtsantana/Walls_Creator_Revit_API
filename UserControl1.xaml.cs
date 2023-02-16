using System;
using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace tarefa2
{
    public partial class UserControl1 : Window
    {
        public UIDocument uidoc { get; }
        public Document doc { get; }

        double altura = 0;

        public UserControl1(UIDocument Uidoc)
        {
            uidoc = Uidoc;
            doc = uidoc.Document;
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IList<Element> WallTypeList = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType().ToElements();

            foreach (Element wall in WallTypeList)
            {
                SelectingWallType.Items.Add(wall.Name);
            }
        }
        public double SetNewHeigh(string text)
        {
            altura = Convert.ToDouble(texthigh.Text) * 3.280839895013123;
            return altura;
        }

        public Element SelectingWallType_DataContextChanged(Document doc)
        {
            IList<Element> WallTypeList = new FilteredElementCollector(doc)
            .OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType().ToElements();

            Element wallTypeSelected = null;
            string wallTypeName = SelectingWallType.Text.ToString();
            foreach (Element wall in WallTypeList)
            {
                if (wall.Name == wallTypeName)
                {
                    wallTypeSelected = wall;
                    break;
                }
                Close();
            }
            return wallTypeSelected;
        }
        private void SetNewHeigh(object sender, RoutedEventArgs e)
        {
        }
        private void SelectingWallType_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

    }
}
