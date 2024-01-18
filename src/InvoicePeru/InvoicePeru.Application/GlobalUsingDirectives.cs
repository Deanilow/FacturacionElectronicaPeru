﻿global using InvoicePeru.Domain.Entities;
global using MediatR;
global using InvoicePeru.Application.Authentication.Common;
global using InvoicePeru.Application.Common.Errors;
global using InvoicePeru.Application.Common.Interfaces.Authentication;
global using InvoicePeru.Application.Common.Interfaces.Persistence;
global using Microsoft.Extensions.DependencyInjection;
global using InvoicePeru.Application.Common.Interfaces.Services;
global using System.Text;
global using System.Text.RegularExpressions;
global using PasswordGenerator;
