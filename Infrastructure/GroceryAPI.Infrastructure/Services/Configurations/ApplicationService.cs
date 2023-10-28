﻿using GroceryAPI.Application.Abstractions.Services.Configurations;
using GroceryAPI.Application.CustomAttributes;
using GroceryAPI.Application.DTOs.Configuration;
using GroceryAPI.Application.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;

namespace GroceryAPI.Infrastructure.Services.Configurations
{
    internal class ApplicationService : IApplicationService
    {
        public List<Menu> GetAuthorizeDefinitionEndpoints(Type type)
        {
            Assembly? assembly = Assembly.GetAssembly(type);
            var controllers = assembly?.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            List<Menu> menus = new();

            if(controllers != null)
            {
                foreach (var controller in controllers)
                {
                    var actions = controller.GetMethods().Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute)));
                    if (actions != null)
                    {
                        foreach (var action in actions)
                        {
                            var attributes = action.GetCustomAttributes(true);
                            if(attributes != null)
                            {
                                Menu menu = null;
                                var authorizedDefinitionAttribute = attributes.FirstOrDefault(a => a.GetType() == typeof(AuthorizeDefinitionAttribute))as AuthorizeDefinitionAttribute;
                                if(!menus.Any(m => m.Name == authorizedDefinitionAttribute.Menu))
                                {
                                    menu = new() { Name = authorizedDefinitionAttribute.Menu };
                                    menus.Add(menu);
                                }
                                else
                                {
                                    menu = menus.FirstOrDefault(m => m.Name == authorizedDefinitionAttribute.Menu);
                                }
                                Application.DTOs.Configuration.Action _action = new()
                                {
                                    ActionType = Enum.GetName(typeof(ActionType),authorizedDefinitionAttribute.ActionType),
                                    Definition = authorizedDefinitionAttribute.Definition
                                };

                                var httpAttribute = attributes.FirstOrDefault(a => a.GetType().IsAssignableTo(typeof(HttpMethodAttribute))) as HttpMethodAttribute;
                                if(httpAttribute != null)
                                {
                                    _action.HttpType = httpAttribute.HttpMethods.First();
                                }
                                else
                                {
                                    _action.HttpType = HttpMethods.Get;
                                }
                                _action.Code = $"{_action.HttpType}.{_action.ActionType}.{_action.Definition.Replace(" ", "")}";
                                menu.Actions.Add(_action);
                            }
                        }
                    }
                }
            }
            return menus;
        }
    }
}