using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using AenEnterprise.ServiceImplementations.Interface;
using AenEnterprise.ServiceImplementations.Mapping;
using AenEnterprise.ServiceImplementations.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Implementation
{
    public class CategoryService : ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IUnitOfWork _unitOfWork;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<GetAllCategoryResponse> GetAll()
        {
            throw new NotImplementedException();
        }

        //GetAllCategoryResponse ICategoryService.GetAll()
        //{
        //    IEnumerable<Category> categories=_categoryRepository.GetCategoryWithProducts();
        //    GetAllCategoryResponse response=new GetAllCategoryResponse();
        //    response.Categories = categories.ConvertToCategoryViews();
        //    return response;
        //}
    }
}
