using System;
using System.Web.Mvc;
using System.Xml;
using XMLEditor.Models;

namespace XMLEditor.Controllers
{
    public class EditorController : Controller
    {
        // GET: Editor
        public ActionResult Index(string fileName)
        {
            try
            {
                ViewBag.fielName = fileName;
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(System.IO.File.ReadAllText(Server.MapPath($"~/uploads/{fileName}.xml")));
                return View(GetModel(xmlDoc.DocumentElement));
            }
            catch (Exception error)
            {
                return View(new XMLNodeModel() { NodeName = "ERROR: " + error.Message });
            }
        }

        [HttpPost]
        public JsonResult Save(string fileName, XMLNodeModel data)
        {
            bool success = true;
            string serverError = null;
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.AppendChild(GetModel(data, xmlDoc));
                xmlDoc.Save(Server.MapPath($"~/uploads/{fileName}.xml"));
            }
            catch (Exception error)
            {
                success = false;
                serverError = error.Message;
            }
            return Json(new { success, serverError }, JsonRequestBehavior.AllowGet);
        }

        private XmlElement GetModel(XMLNodeModel node, XmlDocument xmlDoc)
        {
            var result = xmlDoc.CreateElement(node.NodeName);
            if (node.Attributes?.Count > 0)
            {
                foreach (var attribute in node.Attributes)
                {
                    result.SetAttribute(attribute.Key, attribute.Value);
                }
            }
            if (node.ChildNodes?.Count > 0)
            {
                foreach (var childElement in node.ChildNodes)
                {
                    result.AppendChild(GetModel(childElement, xmlDoc));
                }
            }
            return result;
        }
        private XMLNodeModel GetModel(XmlElement node)
        {
            var result = new XMLNodeModel()
            {
                NodeName = node.Name
            };
            if (node.HasAttributes)
            {
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    result.Attributes.Add(attribute.Name, attribute.Value);
                }
            }
            if (node.HasChildNodes)
            {
                foreach (XmlNode childElement in node.ChildNodes)
                {
                    if (childElement is XmlElement)
                    {
                        result.ChildNodes.Add(GetModel(childElement as XmlElement));
                    }
                }
            }
            return result;
        }
    }
}