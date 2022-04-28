using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore.InMemory;
using GameBlog.Services;
using GameBlog.Test.Mock;
using GameBlog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GameBlog.Models;
using GameBlog.Data;
using System.Threading;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http;

namespace GameBlog.Test.Mock
{
    public class ServiceProvisioner : IClassFixture<CustomWebApplicationFactory>
    {
        private GameBlogDbContext db;
        private ArticlesService articleService;
        private readonly CustomWebApplicationFactory factory;
        private readonly AsyncServiceScope scope;

        public ServiceProvisioner(CustomWebApplicationFactory factor)
        {
            this.factory = factor;
            scope = factory.Services.CreateAsyncScope();
        }
    }
}
