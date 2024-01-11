using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.RequestHelpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : BaseApiController
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public ProductsController(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams productParams)
    {
        var query = _context.Products
            .Sort(productParams.OrderBy)
            .Search(productParams.SearchTerm)
            .AsQueryable();

        var products =
            await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

        Response.AddPaginationHeader(products.MetaData);

        return products;
    }

    [HttpGet("{id}", Name = "GetProduct")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return NotFound();

        return product;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);

        _context.Products.Add(product);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

        return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<Product>> UpdateProduct([FromForm]UpdateProductDto productDto)
    {
        var product = await _context.Products.FindAsync(productDto.Id);

        if (product == null) return NotFound();

        _mapper.Map(productDto, product);
        
        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok(product);

        return BadRequest(new ProblemDetails { Title = "Problem updating product" });
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return NotFound();

        
        _context.Products.Remove(product);

        var result = await _context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
    }
}