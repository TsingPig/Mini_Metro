import math
from PIL import Image, ImageDraw


def create_image(size, color = (0, 0, 0, 0)):
    return Image.new("RGBA", size, color)


def draw_circle(draw, center, radius, fill, outline, width):
    x, y = center
    draw.ellipse((x - radius, y - radius, x + radius, y + radius), fill = fill, outline = outline, width = width)


def draw_rounded_rectangle(draw, top_left, bottom_right, radius, fill, outline, width):
    draw.rounded_rectangle([top_left, bottom_right], radius, fill = fill, outline = outline, width = width)


def draw_triangle(draw, center, size, fill, outline, width):
    x, y = center
    height = size * (3 ** 0.5) / 2  # Height of an equilateral triangle
    points = [(x, y - height / 2), (x - size / 2, y + height / 2), (x + size / 2, y + height / 2)]
    draw.polygon(points, fill = fill, outline = outline, width = width)


def draw_diamond(draw, center, width_height, fill, outline, width):
    x, y = center
    w = height = width_height
    points = [(x, y - height // 2), (x + w // 2, y), (x, y + height // 2), (x - w // 2, y)]
    draw.polygon(points, fill = fill, outline = outline, width = width)




def save_image(image, filename):
    image.save(filename)


# Parameters
size = (400, 400)
center = (200, 200)
radius = 100
color_fill = (0, 0, 0, 255)  # Black fill
color_outline = (0, 0, 0, 255)  # Black outline
width = 23

# # Draw and save circle
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_circle(draw, center, radius, fill=color_fill, outline=None, width=0)
# save_image(img, "circle_solid.png")
#
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_circle(draw, center, radius, fill=None, outline=color_outline, width=width)
# save_image(img, "circle_outline.png")

# # Draw and save rounded rectangle
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_rounded_rectangle(draw, (100, 100), (300, 300), radius=20, fill=color_fill, outline=None, width=0)
# save_image(img, "rounded_rectangle_solid.png")
#
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_rounded_rectangle(draw, (100, 100), (300, 300), radius=20, fill=None, outline=color_outline, width=width)
# save_image(img, "rounded_rectangle_outline.png")

# # Draw and save triangle
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_triangle(draw, center, size=200, fill=color_fill, outline=None, width=0)
# save_image(img, "triangle_solid.png")
#
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_triangle(draw, center, size=200, fill=None, outline=color_outline, width=width)
# save_image(img, "triangle_outline.png")

# # Draw and save diamond
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_diamond(draw, center, 200, fill = color_fill, outline = None, width = 0)
# save_image(img, "diamond_solid.png")
#
# img = create_image(size)
# draw = ImageDraw.Draw(img)
# draw_diamond(draw, center, 200, fill = None, outline = color_outline, width = width)
# save_image(img, "diamond_outline.png")

