using SimpleWorkbench.Api.Application.Search;

namespace SimpleWorkbench.Api.Api.Endpoints;

public static class SearchEndpoints
{
    public static IEndpointRouteBuilder MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/search");

        group.MapGet("/lexical", async (string q, LexicalSearchService service, CancellationToken cancellationToken) =>
        {
            var hits = await service.SearchAsync(q, cancellationToken);
            return Results.Ok(new SearchResponse(hits));
        });

        return app;
    }

    public sealed record SearchResponse(IReadOnlyList<SearchHit> Items);
}
