using System;
using System.Runtime;
using AutoMapper;

namespace Application.Mappings;

public interface IMap
{
    void Mapping(Profile profile);
}
