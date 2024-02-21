// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using MvcMovie.Data;
// using System;
// using System.Linq;
// using App.Models.Blog;

// namespace MvcMovie.Models;

// public static class SeedData
// {
//     public static void Initialize(IServiceProvider serviceProvider)
//     {
//         using (var context = new MvcMovieContext(
//             serviceProvider.GetRequiredService<
//                 DbContextOptions<MvcMovieContext>>()))
//         {
//             // Look for any movies.
//             if (context.Categories.Any())
//             {
//               Console.WriteLine("Exist Db!");
//                 return;   // DB has been seeded
//             }
//             var parentCategoryId = Guid.NewGuid().ToString();
//             var parentCategoryId2 = Guid.NewGuid().ToString();
//             var parentCategoryId3 = Guid.NewGuid().ToString();
//             context.Categories.AddRange(
//                 new Category
//                 {
//                     Id = parentCategoryId,
//                     ParentCategoryId = null,
//                     Title = "C#",
//                     Description = "C# Programming",
//                     Slug = "c-sharp"
//                 },
//                 new Category
//                 {
//                     Id = parentCategoryId2,
//                     ParentCategoryId = parentCategoryId,
//                     Title = "Blazor",
//                     Description = "Blazor web, Server side mix client side",
//                     Slug = "blazor"
//                 },
//                 new Category
//                 {
//                     Id = parentCategoryId3,
//                     ParentCategoryId = parentCategoryId2,
//                     Title = "Razor Page",
//                     Description = "Razor Web, origin for mvc",
//                     Slug = "razor"
//                 },
//                 new Category
//                 {
//                     Id = Guid.NewGuid().ToString(),
//                     ParentCategoryId = parentCategoryId,
//                     Title = "C# Mobile",
//                     Description = "Xamarin or MAUI",
//                     Slug = "maui"
//                 },
//                 new Category
//                 {
//                     Id = Guid.NewGuid().ToString(),
//                     ParentCategoryId = parentCategoryId,
//                     Title = "C# Desktop",
//                     Description = "Desktop app Programming",
//                     Slug = "win-form"
//                 }
//             );
//             context.SaveChanges();
//         }
//     }
// }