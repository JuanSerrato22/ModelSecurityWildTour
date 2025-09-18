using Entity.Model;
using System;
using System.Linq.Expressions;
using Utilities.Specifications;

namespace Business.Specifications
{
    public class ActiveActivitiesSpecification : Specification<Activity>
    {
        public override Expression<Func<Activity, bool>> ToExpression()
        {
            return activity => activity.Active;
        }
    }

    public class ActivitiesByCategorySpecification : Specification<Activity>
    {
        private readonly string _category;

        public ActivitiesByCategorySpecification(string category)
        {
            _category = category ?? throw new ArgumentNullException(nameof(category));
        }

        public override Expression<Func<Activity, bool>> ToExpression()
        {
            return activity => activity.Category.ToLower() == _category.ToLower();
        }
    }

    public class ActivityByIdSpecification : Specification<Activity>
    {
        private readonly int _id;

        public ActivityByIdSpecification(int id)
        {
            _id = id;
        }

        public override Expression<Func<Activity, bool>> ToExpression()
        {
            return activity => activity.Id == _id;
        }
    }

    public class ActivityPriceRangeSpecification : Specification<Activity>
    {
        private readonly decimal _minPrice;
        private readonly decimal _maxPrice;

        public ActivityPriceRangeSpecification(decimal minPrice, decimal maxPrice)
        {
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }

        public override Expression<Func<Activity, bool>> ToExpression()
        {
            return activity => activity.Price >= _minPrice && activity.Price <= _maxPrice;
        }
    }

    public class ValidActivitySpecification : Specification<Activity>
    {
        public override Expression<Func<Activity, bool>> ToExpression()
        {
            return activity => !string.IsNullOrWhiteSpace(activity.Name) &&
                             !string.IsNullOrWhiteSpace(activity.Description) &&
                             activity.Price >= 0;
        }
    }
}