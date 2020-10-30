import sys
from math import cos, radians

print("Hello, Visual Studio")

for i in range(360):
    print(cos(radians(i)))


print("再添加一小段代码以输出 360 度的余弦值：")

for i in range(360):
    print(cos(radians(i)))
print("end 余弦值：")
# Create a string with spaces proportional to a cosine of x in degrees


import sys
from math import cos, radians

# Create a string with spaces proportional to a cosine of x in degrees
def make_dot_string(x):
    return ' ' * int(20 * cos(radians(x)) + 20) + 'o'



#for i in range(360):
#    s = make_dot_string(i)
#    print(s)

for i in range(0, 360, 12):
    s = make_dot_string(i)
    print(s)
for i in range(0, 1080, 12):
    s = make_dot_string(i)
    print(s)