using api_counter.wwwapi8;
using api_counter.wwwapi8.Helpers;
using api_counter.wwwapi8.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

CounterHelper.Initialize();

var counters = app.MapGroup("/counters");
//TODO: 1. write a method that returns all counters in the counters list.  use method below as a starting point
counters.MapGet("/", () =>
{
    return TypedResults.Ok(CounterHelper.Counters);
});


//TODO: 2. write a method to return a single counter based on the id being passed in.  complete method below
counters.MapGet("/{id}", IResult (int id) =>
{
    Counter counter = CounterHelper.Counters.FirstOrDefault(x => x.Id == id);
    return counter != null ? TypedResults.Ok(counter) : TypedResults.NotFound($"Id {id} not found");
});

//TODO: 3.  write another controlller method that returns counters that have a value greater than the {number} passed in.        
counters.MapGet("/greaterthan/{number}", IResult (int number) =>
{
    List<Counter> counter = CounterHelper.Counters.Where(x => x.Value > number).ToList();
    return counter.Any() ? TypedResults.Ok(counter) : 
    TypedResults.NotFound($"No counter with value greater than {number} not found");
});

////TODO:4. write another controlller method that returns counters that have a value less than the {number} passed in.
counters.MapGet("/lessthan/{number}", IResult (int number) =>
{
    List<Counter> counter = CounterHelper.Counters.Where(x => x.Value < number).ToList();
    return counter.Any() ? TypedResults.Ok(counter) :
    TypedResults.NotFound($"No counter with value less than {number} not found");
});


//Extension #1
//TODO:  1. Write a controller method that increments the Value property of a counter of any given Id.
//e.g.  with an Id=1  the Books counter Value should be increased from 5 to 6
//return the counter you have increased
counters.MapPut("/increment/{number}", IResult (int id) =>
{
    Counter counter = CounterHelper.Counters.FirstOrDefault(x => x.Id == id);
    if (counter == null) return TypedResults.NotFound($"Id {id} not found");
    counter.Value++;
    return TypedResults.Ok(counter);
});

//Extension #2
//TODO: 2. Write a controller method that decrements the Value property of a counter of any given Id.
//e.g.  with an Id=1  the Books counter Value should be decreased from 5 to 4
//return the counter you have decreased
counters.MapPut("/decrement/{number}", IResult (int id) =>
{
    Counter counter = CounterHelper.Counters.FirstOrDefault(x => x.Id == id);
    if (counter == null) return TypedResults.NotFound($"Id {id} not found");
    counter.Value--;
    return TypedResults.Ok(counter);
});


app.Run();

