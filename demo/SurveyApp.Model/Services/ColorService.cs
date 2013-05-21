using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using SurveyApp.Model.Models;

namespace SurveyApp.Model.Services
{
    public interface IColorService
    {
        IQueryable<Color> Get();
        void Save(IEnumerable<Color> colors);
    }

    public class ColorService : IColorService
    {
        private readonly IDocumentSession _documentSession;

        public ColorService(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public IQueryable<Color> Get()
        {
            return _documentSession.Query<Color>();
        }

        public void Save(IEnumerable<Color> colors)
        {
            foreach (var color in colors)
                _documentSession.Store(color);
        }
    }
}
