using GameQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameQuiz.Domain.Interfaces;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync();
}
