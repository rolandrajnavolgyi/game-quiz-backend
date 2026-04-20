using System;
using System.Collections.Generic;
using System.Text;

namespace GameQuiz.Domain.Entities;

public abstract class Entity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
}
