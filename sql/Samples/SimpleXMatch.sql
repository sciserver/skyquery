SELECT s.objid, s.ra, s.dec, g.objid, g.ra, g.dec, x.ra, x.dec
INTO clxmatch
FROM SDSSDR7:PhotoObjAll AS s
CROSS JOIN GALEX:PhotoObjAll AS g
XMATCH BAYESFACTOR x
MUST EXIST s ON POINT(s.ra, s.dec), 0.1, 0.1, 0.1
MUST EXIST g ON POINT(g.ra, g.dec), 0.2, 0.2, 0.2
HAVING LIMIT 1e3
WHERE s.ra BETWEEN 0 AND 5 AND s.dec BETWEEN 0 AND 5
    AND g.ra BETWEEN 0 AND 5 AND g.dec BETWEEN 0 AND 5
