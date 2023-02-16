using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace tarefa2
{
    [Transaction(TransactionMode.Manual)]
    public class WallCreator : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Crianção das variáveis necessárias
            var uiapp = commandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var doc = uiapp.ActiveUIDocument.Document;
            var levelId = doc.ActiveView.GenLevel.Id;


            //Criando um filtro para pegar todas as instâncias de Model Line, retirando os tipos de linha
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Lines);
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> lines = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();


            //Inicialização da janela para o input de uma nova altura
            UserControl1 newHigh = new UserControl1(uidoc);
            newHigh.ShowDialog();


            //Indicando o offset das paredes como 0 para que não saiam do local
            double offsetWall = 0;


            //Buscando o Id do elemento selecionado pelo usuário no ComboBox do WPF
            ElementId wallSelectedId = newHigh.SelectingWallType_DataContextChanged(doc).Id;


            //Iniciando a transação para criação das paredes
            Transaction t = new Transaction(doc,"creating new walls");
            t.Start();
            var alturaWalls = newHigh.SetNewHeigh(newHigh.texthigh.Text);
            foreach (Element line in lines)
            {
                CurveElement curve = line as CurveElement;
                XYZ StartPoint = curve.GeometryCurve.GetEndPoint(0);
                XYZ endPoint = curve.GeometryCurve.GetEndPoint(1);
                Curve lineBound = Line.CreateBound(StartPoint, endPoint) as Curve;
                Wall.Create(doc, lineBound, wallSelectedId, levelId, alturaWalls, offsetWall, false, false);
            }
            t.Commit();
            return Result.Succeeded;
        }

    }

}