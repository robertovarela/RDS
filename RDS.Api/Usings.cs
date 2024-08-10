/* add global usage directives for System namespaces here */


global using System.IdentityModel.Tokens.Jwt;
global using System.IO.Compression;
global using System.Net;
global using System.Net.Mail;
global using System.Net.NetworkInformation;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json.Serialization;

/* add global usage directives for Microsoft namespaces here */

global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.ResponseCompression;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.IdentityModel.Tokens;

/* add global usage directives for Comanda namespaces here */

global using RDS.Api.Common.Api;
global using RDS.Api.Data;
global using RDS.Api.Handlers;
global using RDS.Api.Models;
global using RDS.Api.Services;

global using RDS.Core;
global using RDS.Core.Common.Extensions;
global using RDS.Core.Enums;
global using RDS.Core.Handlers;
global using RDS.Core.Models;
global using RDS.Core.Models.Account;
global using RDS.Core.Models.ApplicationUser;
global using RDS.Core.Models.Reports;
global using RDS.Core.Requests.Account;
global using RDS.Core.Requests.ApplicationUsers;
global using RDS.Core.Requests.ApplicationUsers.Address;
global using RDS.Core.Requests.Reports;
global using RDS.Core.Responses;
global using RDS.Core.Services;

/* add global usage directives for Third party namespaces here */

global using Stripe;