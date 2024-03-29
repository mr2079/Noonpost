﻿using Domain.Entites.Category;

namespace Infrastructure.ViewModels;

public class AdminDashboardViewModel
{
    public int AllArticlesCount { get; set; }
    public int AllUsersCount { get; set; }
    public int AllAdminsCount { get; set; }
    public int NewCommentsCount { get; set; }
    public int UnacceptedArticlesCount { get; set; }
}

public class AdminInfoViewModel
{
    public long CId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
}

public class ArticlesWithNewCommentViewModel
{
    public Guid ArticleId { get; set; }
    public long ArticleCId { get; set; }
    public string ArticleTitle { get; set; } = string.Empty;
    public string ArticleImageName { get; set; } = string.Empty;
    public int NewCommentsCount { get; set; }
}

public class CategoryViewModel
{
    public Category Category { get; set; } = new();
    public int CategoryArticlesCount { get; set; } = 0;
}