using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

namespace RevitWPF
{
    public class ExecuteEvent : IExternalEventHandler
    {
        public int enentID = 1;
        public void Execute(UIApplication app)
        {
            switch (enentID)
            {
                case 1:
                    {
                        this.CreateInstance(app); break;
                    }
                default: break;
            }
        }

        public void CreateInstance(UIApplication app)
        {
            Document familyDoc = app.Application.NewFamilyDocument(@"C:\ProgramData\Autodesk\RVT 2017\Family Templates\Chinese\公制常规模型.rft");
            try
            {
                Transaction transaction = new Transaction(familyDoc, "Create family");
                if (transaction.Start() == TransactionStatus.Started)
                {
                    CurveArray curveArray = new CurveArray();
                    curveArray.Append(Line.CreateBound(new XYZ(0, 0, 0), new XYZ(5, 0, 0)));
                    curveArray.Append(Line.CreateBound(new XYZ(5, 0, 0), new XYZ(5, 5, 0)));
                    curveArray.Append(Line.CreateBound(new XYZ(5, 5, 0), new XYZ(0, 5, 0)));
                    curveArray.Append(Line.CreateBound(new XYZ(0, 5, 0), new XYZ(0, 0, 0)));
                    CurveArrArray curveArrArray = new CurveArrArray();
                    curveArrArray.Append(curveArray);
                    //创建一个拉伸实体
                    familyDoc.FamilyCreate.NewExtrusion(true, curveArrArray, SketchPlane.Create(familyDoc, Plane.CreateByNormalAndOrigin(new XYZ(0, 0, 1), XYZ.Zero)), 10);
                    //创建一个族类型
                    familyDoc.FamilyManager.NewType("MyNewType");
                    transaction.Commit();
                    familyDoc.SaveAs(@"C:\Users\Administrator\Desktop\MyNewFamily.rfa");
                    familyDoc.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //估计是记录外部事件名称的 和事务名称相同
        public string GetName()
        {
            return "this is a Test";
        }

    }
}
