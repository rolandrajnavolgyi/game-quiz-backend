using GameQuiz.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameQuiz.Application.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameDTO>> GetAllAsync();
}
