using MyChy.Frame.Core.Common.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyChy.Frame.Core.EFCore.Entitys.Pages
{
    // codehint: sm-add (whole file)
    public abstract class PagedListBase : IPageable
    {

        protected PagedListBase()
        {
            this.PageIndex = 0;
            this.PageSize = 0;
            this.TotalCount = 1;
        }

        protected PagedListBase(IPageable pageable)
        {
            this.Init(pageable);
        }

        protected PagedListBase(int pageIndex, int pageSize, int totalItemsCount)
        {

            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalItemsCount;
        }

        // only here for compat reasons with nc
        public void LoadPagedList<T>(IPagedList<T> pagedList)
        {
            this.Init(pagedList as IPageable);
        }

        public virtual void Init(IPageable pageable)
        {

            this.PageIndex = pageable.PageIndex;
            this.PageSize = pageable.PageSize;
            this.TotalCount = pageable.TotalCount;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageNumber
        {
            get
            {
                return this.PageIndex + 1;
            }
            set
            {
                this.PageIndex = value - 1;
            }
        }

        public int TotalPages
        {
            get
            {
                var total = (this.PageSize == 0 ? 0 : this.TotalCount / this.PageSize);

                if (this.TotalCount % this.PageSize > 0)
                    total++;

                return total;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return this.PageIndex > 0;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (this.PageIndex < (this.TotalPages - 1));
            }
        }

        public int FirstItemIndex
        {
            get
            {
                return (this.PageIndex * this.PageSize) + 1;
            }
        }

        public int LastItemIndex
        {
            get
            {
                return Math.Min(this.TotalCount, ((this.PageIndex * this.PageSize) + this.PageSize));
            }
        }

        public bool IsFirstPage
        {
            get
            {
                return (this.PageIndex <= 0);
            }
        }

        public bool IsLastPage
        {
            get
            {
                return (this.PageIndex >= (this.TotalPages - 1));
            }
        }

        public virtual IEnumerator GetEnumerator()
        {
            return Enumerable.Empty<int>().GetEnumerator();
        }

    }
    public class PagedList : PagedListBase
    {
        public PagedList(int pageIndex, int pageSize, int totalItemsCount)
            : base(pageIndex, pageSize, totalItemsCount)
        {
        }
    }

    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {

            Init(source.Skip(pageIndex * pageSize).Take(pageSize), pageIndex, pageSize, source.Count());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {

            Init(source.Skip(pageIndex * pageSize).Take(pageSize), pageIndex, pageSize, source.Count);
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            // codehint: sm-edit

            Init(source, pageIndex, pageSize, totalCount);
        }

        // codehint: sm-add
        private void Init(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {


            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;

            this.AddRange(source);
        }

        #region IPageable Members

        // codehint: sm-add/edit

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageNumber
        {
            get
            {
                return this.PageIndex + 1;
            }
            set
            {
                this.PageIndex = value - 1;
            }
        }

        public int TotalPages
        {
            get
            {
                var total = this.TotalCount / this.PageSize;

                if (this.TotalCount % this.PageSize > 0)
                    total++;

                return total;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return this.PageIndex > 0;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (this.PageIndex < (this.TotalPages - 1));
            }
        }

        public int FirstItemIndex
        {
            get
            {
                return (this.PageIndex * this.PageSize) + 1;
            }
        }

        public int LastItemIndex
        {
            get
            {
                return Math.Min(this.TotalCount, ((this.PageIndex * this.PageSize) + this.PageSize));
            }
        }

        public bool IsFirstPage
        {
            get
            {
                return (this.PageIndex <= 0);
            }
        }

        public bool IsLastPage
        {
            get
            {
                return (this.PageIndex >= (this.TotalPages - 1));
            }
        }

        #endregion

    }
}
