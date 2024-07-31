import os
from PIL import Image, ImageDraw


class ShapeDrawer:
    def __init__(self, size, background_color=(0, 0, 0, 0)):
        self.size = size
        self.image = Image.new("RGBA", size, background_color)
        self.draw = ImageDraw.Draw(self.image)

    def save(self, filename):
        self.image.save(filename)

    def draw_circle(self, center, radius, fill=None, outline=None, width=1):
        x, y = center
        self.draw.ellipse((x - radius, y - radius, x + radius, y + radius),
                          fill=fill, outline=outline, width=width)

    def draw_rounded_rectangle(self, top_left, bottom_right, radius, fill=None, outline=None, width=1):
        self.draw.rounded_rectangle([top_left, bottom_right], radius, fill=fill, outline=outline, width=width)

    def draw_triangle(self, center, size, fill=None, outline=None, width=1):
        x, y = center
        height = size * (3 ** 0.5) / 2  # Height of an equilateral triangle
        points = [(x, y - height / 2), (x - size / 2, y + height / 2), (x + size / 2, y + height / 2)]
        self.draw.polygon(points, fill=fill, outline=outline, width=width)

    def draw_diamond(self, center, width_height, fill=None, outline=None, width=1):
        x, y = center
        w = h = width_height
        points = [(x, y - h // 2), (x + w // 2, y), (x, y + h // 2), (x - w // 2, y)]
        self.draw.polygon(points, fill=fill, outline=outline, width=width)




def main():
    # Parameters
    width = 200
    size = (width, width)
    center = (size[0] // 2, size[1] // 2)
    radius = min(size) // 2 - 20
    color_fill = (255, 255, 255, 255)
    color_outline = (255, 255, 255, 255)
    width = width // 15

    shapes = [
        ("circle_solid.png", "circle", {'center': center, 'radius': radius, 'fill': color_fill}),
        ("circle_outline.png", "circle", {'center': center, 'radius': radius, 'outline': color_outline, 'width': width}),
        ("rounded_rectangle_solid.png", "rounded_rectangle", {'top_left': (20, 20), 'bottom_right': (size[0] - 20, size[1] - 20), 'radius': 20, 'fill': color_fill}),
        ("rounded_rectangle_outline.png", "rounded_rectangle", {'top_left': (20, 20), 'bottom_right': (size[0] - 20, size[1] - 20), 'radius': 20, 'outline': color_outline, 'width': width}),
        ("triangle_solid.png", "triangle", {'center': center, 'size': size[0] - 40, 'fill': color_fill}),
        ("triangle_outline.png", "triangle", {'center': center, 'size': size[0] - 40, 'outline': color_outline, 'width': width}),
        ("diamond_solid.png", "diamond", {'center': center, 'width_height': size[0] - 40, 'fill': color_fill}),
        ("diamond_outline.png", "diamond", {'center': center, 'width_height': size[0] - 40, 'outline': color_outline, 'width': width}),
    ]

    save_directory = os.path.join("..", "..", "Res", "Texture", "CityNodeTexture")
    os.makedirs(save_directory, exist_ok=True)

    for filename, shape, params in shapes:
        drawer = ShapeDrawer(size)
        draw_method = getattr(drawer, f'draw_{shape}')
        draw_method(**params)
        save_path = os.path.join(save_directory, filename)
        drawer.save(save_path)


if __name__ == "__main__":
    main()

