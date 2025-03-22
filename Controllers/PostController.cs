using Microsoft.AspNetCore.Mvc;
using WattsFlowProject.Models;
using WattsFlowProject.BusinessLogic.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WattsFlowProject.Controllers
{
    [Authorize]

    public class PostController : BaseController
    {
        private readonly PostRepository _postRepository;

        public PostController(PostRepository postRepository, SystemSettingsRepository settingsRepository) : base(settingsRepository)
        {
            _postRepository = postRepository;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var posts = await _postRepository.GetPostsAsync();
                if (posts == null)
                {
                    posts = new List<PostDetails>(); // Return an empty list if no posts are found
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء جلب العلب: " + ex.Message;
                return View(new List<PostDetails>()); // Return an empty list on error
                                                      // Ensure an empty list is passed in case of an exception
            }
        }


        // GET: /Post/Create
        public async Task<IActionResult> Create()
        {
            return View(new PostDetails()); // Passing an empty instance of PostDetails for creation
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostDetails post)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, we can directly fetch the expense types
                ViewBag.ExpenseTypes = await _postRepository.GetPostsAsync();
                return View(post);
            }
            var existingpost = (await _postRepository.GetPostsAsync())
               .FirstOrDefault(it => it.PostNumber == post.PostNumber);

            if (existingpost != null)
            {
                ModelState.AddModelError("Post", "رقم العلبة هذا موجود بالفعل.");
                return View(post);
            }

            try
            {
                post.IsDeleted = false;
                await _postRepository.CreatePostAsync(post);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء حفظ العلبة: " + ex.Message;
                return View(post);
            }
        }

        // GET: /Post/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var post = (await _postRepository.GetPostsAsync())
                .FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: /Post/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostDetails post)
        {
            if (id != post.PostId)
            {
                ModelState.AddModelError("PostId", "معرف العلبة غير صالح.");
                return View(post);
            }

            if (!ModelState.IsValid)
            {
                return View(post);
            }
            var existingPost = (await _postRepository.GetPostsAsync())
               .FirstOrDefault(et => et.PostNumber == post.PostNumber && et.PostId != post.PostId);

            if (existingPost != null)
            {
                ModelState.AddModelError("PostNumber", "رقم العلبة هذا موجود بالفعل.");
                return View(post);
            }

            try
            {
                bool isUpdated = await _postRepository.EditPostAsync(post);
                if (!isUpdated)
                {
                    ModelState.AddModelError("PostId", "العلبة غير موجودة.");
                    return View(post);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء تحديث العلبة: " + ex.Message);
                return View(post);
            }
        }

        // GET: /Post/Delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool isDeleted = await _postRepository.DeletePostAsync(id);
                if (!isDeleted)
                {
                    ViewBag.ErrorMessage = "العلبة غير موجودة أو فشل الحذف.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "حدث خطأ أثناء حذف العلبة: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
