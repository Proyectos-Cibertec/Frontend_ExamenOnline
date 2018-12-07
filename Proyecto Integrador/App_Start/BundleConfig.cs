using System;
using System.Web.Optimization;

namespace Proyecto_Integrador.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {

            bundles.Add(new ScriptBundle("~/bundles/AngularJs").Include(
                "~/Scripts/angular.js",
                "~/Scripts/app/Module/Module.js",
                "~/Scripts/app/Controller/ResolverExamenController.js",
                "~/Scripts/app/Controller/ListadoExamenController.js",
                "~/Scripts/app/Controller/ComprarController.js"
                ));

        }
    }
}