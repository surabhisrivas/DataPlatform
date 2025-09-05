using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

public class RoutePrefixConvention : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix;

    public RoutePrefixConvention(string prefix)
    {
        _routePrefix = new AttributeRouteModel(new RouteAttribute(prefix));
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            // Skip OData controllers (optional: use a namespace or marker attribute)
            if (controller.ControllerType.Namespace?.Contains("Microsoft.AspNetCore.OData") == true)
                continue;

            foreach (var selector in controller.Selectors)
            {
                var attrRoute = selector.AttributeRouteModel;

                //// Skip absolute routes starting with ~
                //if (attrRoute != null && attrRoute.Template?.StartsWith("~") == true)
                //    continue;

                if (attrRoute != null)
                {
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, attrRoute);
                }
                else
                {
                    selector.AttributeRouteModel = _routePrefix;
                }
            }
        }
    }

}

