﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Linq;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using SportsStore.Infrastructure;
using SportsStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class PageLinkTagHelperTests
    {
        [Fact]
        public void CanGeneratePageLings()
        {
            var urlhelper = new Mock<IUrlHelper>();
            urlhelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("Test/Page1")
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlhelper.Object);

            PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object) //класс, который проверяем (создает div'ы)
            {
                PageModel = new PagingInfo //ViewModel для отображения страниц
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                },
                PageAction = "Test" //имя метода для URL
            };
            TagHelperContext ctx = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
            var content = new Mock<TagHelperContent>();
            TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));

            helper.Process(ctx, output); // генерируем страницы

            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
                       + @"<a href=""Test/Page2"">2</a>"
                       + @"<a href=""Test/Page3"">3</a>",
                         output.Content.GetContent());
        }
    }
}