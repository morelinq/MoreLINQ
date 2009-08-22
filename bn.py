import sys
from time import gmtime
year, mon, mday, hour, min, sec, wday, yday, isdst = gmtime()
bld = ((year - 2000) * 12 + mon - 1) * 100 + mday
rev = hour * 100 + min
print 'Your build and revision number for today is %d.%d.' % (bld, rev)
