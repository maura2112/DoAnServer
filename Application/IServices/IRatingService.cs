using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IRatingService
    {
        Task<RatingDTO> CreateRating(RatingDTO dto);

        Task<List<RatingDTO>> GetRatingsForUser(int uid);
    }
}
