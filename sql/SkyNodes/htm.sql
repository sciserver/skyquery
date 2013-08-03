SELECT p.*, c.x cx, c.y cy, c.z cz,
      SphRegion.dbo.fHtmEq(p.ra, p.dec) AS htmid
FROM PhotoObjAll p
CROSS APPLY SphRegion.dbo.fHtmEqToXyz(p.ra, p.dec) c