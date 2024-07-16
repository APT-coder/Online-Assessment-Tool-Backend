﻿using AutoMapper;
using OnlineAssessmentTool.Models.DTO;
using OnlineAssessmentTool.Models;

namespace OnlineAssessmentTool
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Batch, CreateBatchDTO>().ReverseMap();
            CreateMap<Batch, UpdateBatchDTO>().ReverseMap();

            CreateMap<AssessmentDTO, Assessment>().ReverseMap();
            CreateMap<QuestionDTO, Question>().ReverseMap();
            CreateMap<QuestionOptionDTO, QuestionOption>().ReverseMap();
        }
    }
}