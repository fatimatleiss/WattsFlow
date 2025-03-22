using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WattsFlowProject.Data;
using WattsFlowProject.Models;

namespace WattsFlowProject.BusinessLogic.Repositories
{
    public class PostRepository
    {
        private readonly WattsFlowDbContext _context;

        public PostRepository(WattsFlowDbContext context)
        {
            _context = context;
        }

         public async Task<List<PostDetails>> GetPostsAsync()
        {
            try
            {
                return await _context.Details.Where(x => x.IsDeleted != true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء جلب العلب: " + ex.Message);
            }
        }

         public async Task CreatePostAsync(PostDetails post)
        {
            try
            {
                _context.Details.Add(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء حفظ العلبة: " + ex.Message);
            }
        }

         public async Task<bool> EditPostAsync(PostDetails updatedPost)
        {
            try
            {
                var existingPost = await _context.Details
                    .FirstOrDefaultAsync(p => p.PostId == updatedPost.PostId);

                if (existingPost == null)
                {
                    return false;
                }

                existingPost.PostNumber = updatedPost.PostNumber;
                existingPost.PostAddress = updatedPost.PostAddress;
                _context.Details.Update(existingPost);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء تحديث العلبة: " + ex.Message);
            }
        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            try
            {
                var postToDelete = await _context.Details
                    .FirstOrDefaultAsync(p => p.PostId == postId);

                if (postToDelete == null)
                {
                    return false;
                }
                _context.Details.Remove(postToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("حدث خطأ أثناء حذف العلبة: " + ex.Message);
            }
        }
    }
}
