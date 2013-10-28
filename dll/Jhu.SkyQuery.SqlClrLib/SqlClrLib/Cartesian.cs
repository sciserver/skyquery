using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Jhu.SkyQuery.SqlClrLib
{
    public struct Cartesian
    {
        #region Private members x, y, z
        private double x, y, z;
        #endregion
        #region Public properties

        /// <summary>
        /// Gets or sets the X coordinate of the vector.
        /// </summary>
        [XmlAttribute]
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Gets or sets the Y coordinate of the vector.
        /// </summary>
        [XmlAttribute]
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Gets or sets the Z coordinate of the vector.
        /// </summary>
        [XmlAttribute]
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public double Norm
        {
            get { return Math.Sqrt(this.Norm2); }
        }

        /// <summary>
        /// Gets the square of the length of the vector.
        /// </summary>
        public double Norm2
        {
            get { return x * x + y * y + z * z; }
        }

        /// <summary>
        /// Gets the angular coordinate RA, 
        /// which is computed on the fly.
        /// </summary>
        public double RA
        {
            get
            {
                double ra, dec;
                Xyz2Radec(x, y, z, out ra, out dec);
                return ra;
            }
        }

        /// <summary>
        /// Gets the angular coordinate Dec, 
        /// which is computed on the fly.
        /// </summary>
        public double Dec
        {
            get
            {
                if (double.IsNaN(z))
                {
                    return double.NaN;
                }
                else if (Math.Abs(z) < 1)
                {
                    return Math.Asin(z) * Constants.Radian2Degree;
                }
                else
                {
                    return 90.0 * Math.Sign(z);
                }
            }
        }

        #endregion
        #region Constructors of various kinds

        /// <summary>
        /// Creates a vector from coordinates (x,y,z).
        /// </summary>
        /// <param name="x">Coordinate of vector</param>
        /// <param name="y">Coordinate of vector</param>
        /// <param name="z">Coordinate of vector</param>
        /// <param name="normalize">The flag determines whether vector is normalized.</param>
        public Cartesian(double x, double y, double z, bool normalize)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            if (normalize)
            {
                this.Normalize();
            }
        }
        /// <summary>
        /// Creates a vector from another instance.
        /// </summary>
        /// <param name="p">The input vector</param>
        /// <param name="normalize">The flag determines whether vector is normalized.</param>
        public Cartesian(Cartesian p, bool normalize)
            : this(p.x, p.y, p.z, normalize)
        {
        }

        /// <summary>
        /// Creates a unit vector from angular coordinates (RA, Dec).
        /// </summary>
        /// <param name="ra">Right Ascension (in degrees)</param>
        /// <param name="dec">Declination (in degrees)</param>
        /// <remarks>The equations of the conversion are
        /// <code>
        /// x = Cos(Dec)*Cos(RA)
        /// y = Cos(Dec)*Sin(RA)
        /// z = Sin(Dec)
        /// </code>
        /// </remarks>
        public Cartesian(double ra, double dec)
        {
            Cartesian.Radec2Xyz(ra, dec, out x, out y, out z);
        }

        #endregion
        #region Get methods
        /// <summary>
        /// Get the ith component of this vector.
        /// </summary>
        /// <param name="i">the index of the desired component</param>
        /// <returns>value of the ith component</returns>
        public double Get(int i)
        {
            switch (i)
            {
                case 0: return this.x;
                case 1: return this.y;
                case 2: return this.z;
                default: throw new IndexOutOfRangeException("Cartesian");

            }
        }
        #endregion
        #region Set methods

        /// <summary>
        /// Sets the vector from another.
        /// </summary>
        /// <param name="v">Input vector</param>
        /// <param name="normalize">Flag determines whether the vector should be normalized</param>
        public void Set(Cartesian v, bool normalize)
        {
            Set(v.x, v.y, v.z, normalize);
        }

        /// <summary>
        /// Sets the vector from (x,y,z)
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="z">The Z coordinate.</param>
        /// <param name="normalize">Flag determines whether the vector should be normalized</param>
        public void Set(double x, double y, double z, bool normalize)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            if (normalize)
            {
                this.Normalize();
            }
        }

        /// <summary>
        /// Sets the vector to point from angular position (RA,Dec).
        /// </summary>
        /// <param name="ra">Right Ascension</param>
        /// <param name="dec">Declination</param>
        /// <remarks>The equations of the conversion are
        /// <code>
        /// x = Cos(Dec)*Cos(RA)
        /// y = Cos(Dec)*Sin(RA)
        /// z = Sin(Dec)
        /// </code>
        /// </remarks>
        public void Set(double ra, double dec)
        {
            Cartesian.Radec2Xyz(ra, dec, out x, out y, out z);
        }

        /// <summary>
        /// Sets the vector to be the sum of the specified points.
        /// </summary>
        /// <param name="p">Input vector</param>
        /// <param name="q">Input vector</param>
        /// <param name="normalize">Flag determines whether the result is normalized</param>
        public void SetMiddlePoint(Cartesian p, Cartesian q, bool normalize)
        {
            this.x = p.x + q.x;
            this.y = p.y + q.y;
            this.z = p.z + q.z;
            if (normalize)
            {
                this.Normalize();
            }
        }

        #endregion
        #region Operations on self

        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        /// <returns>The original length of the vector</returns>
        public double Normalize()
        {
            double lenSqr = x * x + y * y + z * z;
            double err = lenSqr - 1;
            double len = 1;

            if (err > Constants.DoublePrecision4x || err < -Constants.DoublePrecision4x)
            {
                len = Math.Sqrt(lenSqr);
                x /= len;
                y /= len;
                z /= len;
                return len;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Mirrors the vector.
        /// </summary>
        /// <remarks>Coordinates are multiplied by -1.</remarks>
        public void Mirror()
        {
            this.x *= -1;
            this.y *= -1;
            this.z *= -1;
        }

        /// <summary>
        /// Returns a mirrored vector.
        /// </summary>
        public Cartesian Mirrored()
        {
            return new Cartesian(-x, -y, -z, false);
        }

        /// <summary>
        /// Multiplies the coordinates by the specified amount.
        /// </summary>
        /// <param name="s">The multiplication factor.</param>
        public void Scale(double s)
        {
            this.x *= s;
            this.y *= s;
            this.z *= s;
        }

        /// <summary>
        /// Returns a vector that is scaled by the specified amount.
        /// </summary>
        /// <param name="s">The multiplication factor.</param>
        /// <returns>The scaled vector.</returns>
        public Cartesian Scaled(double s)
        {
            Cartesian r = this;
            r.Scale(s);
            return r;
        }

        /// <summary>
        /// Returns the tangent vectors pointing to west and north.
        /// </summary>
        /// <param name="west">The west vector.</param>
        /// <param name="north">The north vector.</param>
        public void Tangent(out Cartesian west, out Cartesian north)
        {
            double ra, dec, sinRa, cosRa, sinDec, cosDec;

            Cartesian.Cartesian2RadecRadian(this, out ra, out dec);
            sinRa = Math.Sin(ra);
            cosRa = Math.Cos(ra);
            sinDec = Math.Sin(dec);
            cosDec = Math.Cos(dec);

            west = new Cartesian(sinRa, -cosRa, 0, false);
            north = new Cartesian(-sinDec * cosRa, -sinDec * sinRa, cosDec, false);
        }

        #endregion
        #region Geometrical relations

        /// <summary>
        /// Computes the 3D dot product of the two vectors
        /// </summary>
        /// <param name="p">Input vector</param>
        /// <returns>Dot product</returns>
        public double Dot(Cartesian p)
        {
            return x * p.x + y * p.y + z * p.z;
        }

        /// <summary>
        /// Returns middle point of two points (this and that)
        /// </summary>
        /// <param name="that">The other input point</param>
        /// <param name="normalize">Flag determines whether the result is normalized</param>
        /// <returns>Middle point</returns>
        //-- <remarks>The new point computed as m = (p+q) / |p+q|</remarks>
        public Cartesian GetMiddlePoint(Cartesian that, bool normalize)
        {
            return new Cartesian(x + that.x, y + that.y, z + that.z, normalize);
        }

        /// <summary>
        /// Adds another vector 
        /// </summary>
        /// <param name="that">Input vector</param>
        /// <returns>Sum of the vectors (this and that)</returns>
        public Cartesian Add(Cartesian that)
        {
            return new Cartesian(x + that.x, y + that.y, z + that.z, false);
        }

        /// <summary>
        /// Subtracts another vector
        /// </summary>
        /// <param name="that">Input vector</param>
        /// <returns>Result</returns>
        public Cartesian Sub(Cartesian that)
        {
            return new Cartesian(x - that.x, y - that.y, z - that.z, false);
        }

        /// <summary>
        /// Computes the cross product with another vector
        /// </summary>
        /// <param name="p">Input vector</param>
        /// <param name="normalize">Flag determines whether the result is normalized</param>
        /// <returns>Cross product</returns>
        public Cartesian Cross(Cartesian p, bool normalize)
        {
            return new Cartesian(
                y * p.z - z * p.y,
                z * p.x - x * p.z,
                x * p.y - y * p.x,
                normalize);
        }

        /// <summary>
        /// Calculates the angle to the specified point on the unit sphere
        /// </summary>
        /// <param name="p">Input unit vector</param>
        /// <returns>Angle in radians</returns>
        public double AngleInRadian(Cartesian p)
        {
            double a, d;
            d = 0.5 * Distance(p);
            if (d < 1)
            {
                a = 2 * Math.Asin(d);
            }
            else
            {
                a = Math.PI;
            }
            return a;
        }

        /// <summary>
        /// Calculates Euclidean distance between two points.
        /// </summary>
        /// <param name="p">The other vector.</param>
        /// <returns>The distance.</returns>
        public double Distance(Cartesian p)
        {
            double dx, dy, dz;
            dx = x - p.x;
            dy = y - p.y;
            dz = z - p.z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        /// <summary>
        /// Calculates the angle to the specified point on the unit sphere
        /// </summary>
        /// <param name="p">Input unit vector</param>
        /// <returns>Angle in degrees</returns>
        public double AngleInDegree(Cartesian p)
        {
            return AngleInRadian(p) * Constants.Radian2Degree;
        }

        /// <summary>
        /// Calculates the angle to the specified point on the unit sphere
        /// </summary>
        /// <param name="p">Input unit vector</param>
        /// <returns>Angle in arcminutes</returns>
        public double AngleInArcmin(Cartesian p)
        {
            return AngleInRadian(p) * Constants.Radian2Arcmin;
        }

        /// <summary>
        /// Determines whether two unit vectors are identical.
        /// </summary>
        /// <param name="that">The unit vector to test.</param>
        /// <returns></returns>
        public bool Same(Cartesian that)
        {
            if (this == that) return true;

            // quick reject based on cosine
            if (this.Dot(that) < Constants.CosSafe) return false;

            if (this.AngleInRadian(that) < Constants.Tolerance) return true;
            else return false;
#if false
            // true test based on sine a'la Halfspace.GetXline...
            Cartesian xdir = this.Cross(that, false);
            Cartesian dir2 = new Cartesian(xdir.X * xdir.X, xdir.Y * xdir.Y, xdir.Z * xdir.Z, false);
            double zero = Constants.XLineThreshold;
            if (Math.Max(dir2.x, Math.Max(dir2.y, dir2.z)) > zero) return false;
            else return true;
#endif
        }

#if false
        /// <summary>
        /// Determines whether the point is identical to another.
        /// </summary>
        /// <param name="p">The vector to compare</param>
        /// <param name="epsilon">The accuracy limit.</param>
        /// <returns>Boolean</returns>
        public bool Same_delta(Cartesian p, double epsilon)
        {
            bool eq = false;
            double dx = x - p.x;
            double dy = y - p.y;
            double dz = z - p.z;
            if (Math.Abs(dx) < epsilon && Math.Abs(dy) < epsilon && Math.Abs(dz) < epsilon)
            {
                eq = true;
            }
            return eq;
        }

        /// <summary>
        /// Determines whether the point is identical to another.
        /// </summary>
        /// <param name="p">The vector to compare.</param>
        /// <param name="epsilon">The accuracy limit.</param>
        public bool Same_dist(Cartesian p, double epsilon)
        {
            bool eq = false;
            double dx = x - p.x;
            double dy = y - p.y;
            double dz = z - p.z;
            double dot = dx * dx + dy * dy + dz * dz;
            if ((dx == 0 && dy == 0 && dz == 0) || (dot <= epsilon))
            {
                eq = true;
            }
            return eq;
        }
#endif

#if false
        /// <summary>
        /// Tests whether this vector is the opposite of another vector,
        /// meaning same magnitude but opposite direction
        /// </summary>
        /// <param name="p">The other vector</param>
        /// <param name="epsilon">accuracy</param>
        /// <returns>true if other vector is close enough to 
        /// being opposite, false otherwise</returns>
        public bool Opposite(Cartesian p, double epsilon)
        {
            if (Math.Abs(p.x + x) > epsilon 
             || Math.Abs(p.y + y) > epsilon
             || Math.Abs(p.z + z) > epsilon)
                return false;
            return true;
        }

        /// <summary>
        /// Test whether or not this vector is the opposite
        /// of another vector within Const.Epsilon
        /// Opposite means same magnitude, opposite direction.
        /// </summary>
        /// <param name="p">The other vector</param>
        /// <returns>true if other vector is close enough to 
        /// being opposite, false otherwise</returns>
        public bool Opposite(Cartesian p)
        {
            return this.Opposite(p, Const.Epsilon);
        }
#endif

        #endregion
        #region Static methods

        /// <summary>
        /// Computes the triple product of three vectors.
        /// </summary>
        /// <remarks>The triple product is the volume of the parallelepipedon defined by the input vectors.</remarks>
        /// <param name="p1">Input vector 1.</param>
        /// <param name="p2">Input vector 2.</param>
        /// <param name="p3">Input vector 3.</param>
        /// <returns>The triple product.</returns>
        public static double TripleProduct(Cartesian p1, Cartesian p2, Cartesian p3)
        {
            double x, y, z; // cross product of 1 and 2
            x = p1.y * p2.z - p1.z * p2.y;
            y = p1.z * p2.x - p1.x * p2.z;
            z = p1.x * p2.y - p1.y * p2.x;
            return x * p3.x + y * p3.y + z * p3.z;
        }

        /// <summary>
        /// Converts normalized (x, y, z) coordinates to (RA, Dec) angles.
        /// </summary>
        /// <param name="x">Coordinate</param>
        /// <param name="y">Coordinate</param>
        /// <param name="z">Coordinate</param>
        /// <param name="ra">Right Ascension in degrees (out)</param>
        /// <param name="dec">Declination in degrees (out)</param>
        public static void Xyz2Radec(double x, double y, double z, out double ra, out double dec)
        {
            double epsilon = Constants.DoublePrecision2x;

            double rdec;
            if (z >= 1) rdec = Math.PI / 2;
            else if (z <= -1) rdec = -Math.PI / 2;
            else rdec = Math.Asin(z);
            dec = rdec * Constants.Radian2Degree;

            double cd = Math.Cos(rdec);
            if (cd > epsilon || cd < -epsilon)  // is the vector pointing to the poles?
            {
                if (y > epsilon || y < -epsilon)  // is the vector in the x-z plane?
                {
                    double arg = x / cd;
                    double acos;
                    if (arg <= -1)
                    {
                        acos = 180;
                    }
                    else if (arg >= 1)
                    {
                        acos = 0;
                    }
                    else
                    {
                        acos = Math.Acos(arg) * Constants.Radian2Degree;
                    }
                    if (y < 0.0)
                    {
                        ra = 360 - acos;
                    }
                    else
                    {
                        ra = acos;
                    }
                }
                else
                {
                    ra = (x < 0.0 ? 180.0 : 0.0);
                }
            }
            else
            {
                ra = 0.0;
            }
            return;
        }

        /// <summary>
        /// Converts normalized (x, y, z) coordinates to (RA, Dec) angles.
        /// </summary>
        /// <param name="x">Coordinate</param>
        /// <param name="y">Coordinate</param>
        /// <param name="z">Coordinate</param>
        /// <param name="ra">Right Ascension in radians (out)</param>
        /// <param name="dec">Declination in radians (out)</param>
        public static void Xyz2RadecRadian(double x, double y, double z, out double ra, out double dec)
        {
            double epsilon = Constants.DoublePrecision2x;

            double rdec;
            if (z >= 1) rdec = Math.PI / 2;
            else if (z <= -1) rdec = -Math.PI / 2;
            else rdec = Math.Asin(z);
            dec = rdec; // *Constants.Radian2Degree;

            double cd = Math.Cos(rdec);
            if (cd > epsilon || cd < -epsilon)  // is the vector pointing to the poles?
            {
                if (y > epsilon || y < -epsilon)  // is the vector in the x-z plane?
                {
                    double arg = x / cd;
                    double acos;
                    if (arg <= -1)
                    {
                        acos = Math.PI;
                    }
                    else if (arg >= 1)
                    {
                        acos = 0;
                    }
                    else
                    {
                        acos = Math.Acos(arg);
                    }
                    if (y < 0.0)
                    {
                        ra = 2 * Math.PI - acos;
                    }
                    else
                    {
                        ra = acos;
                    }
                }
                else
                {
                    ra = (x < 0.0 ? Math.PI : 0.0);
                }
            }
            else
            {
                ra = 0.0;
            }
        }

        /// <summary>
        /// Converts (RA, Dec) to (x, y, z).
        /// </summary>
        /// <param name="ra">Right Ascension in degrees</param>
        /// <param name="dec">Declination in degrees</param>
        /// <param name="x">Coordinate (out)</param>
        /// <param name="y">Coordinate (out)</param>
        /// <param name="z">Coordinate (out)</param>
        /// <remarks>The equations of the conversion are essentially as below
        /// <code>
        /// x = Cos(Dec)*Cos(RA)
        /// y = Cos(Dec)*Sin(RA)
        /// z = Sin(Dec)
        /// </code>
        /// </remarks>
        public static void Radec2Xyz(double ra, double dec, out double x, out double y, out double z)
        {
            double epsilon = Constants.DoublePrecision2x;

            double diff;
            double cd = Math.Cos(dec * Constants.Degree2Radian);
            diff = 90.0 - dec;
            // First, compute Z, consider cases, where declination is almost
            // +/- 90 degrees
            if (diff < epsilon && diff > -epsilon)
            {
                x = 0.0;
                y = 0.0;
                z = 1.0;
                return;
            }
            diff = -90.0 - dec;
            if (diff < epsilon && diff > -epsilon)
            {
                x = 0.0;
                y = 0.0;
                z = -1.0;
                return;
            }
            z = Math.Sin(dec * Constants.Degree2Radian);
            //
            // If we get here, then 
            // at least z is not singular
            //
            double quadrant;
            double qint;
            int iint;
            quadrant = ra / 90.0; // how close is it to an integer?
            // if quadrant is (almost) an integer, force x, y to particular
            // values of quad:
            // quad,   (x,y)
            // 0       (1,0)
            // 1,      (0,1)
            // 2,      (-1,0)
            // 3,      (0,-1)
            // q>3, make q = q mod 4, and reduce to above
            // q mod 4 should be 0.
            qint = (double)((int)quadrant);
            if (Math.Abs(qint - quadrant) < epsilon)
            {
                iint = (int)qint;
                iint %= 4;
                if (iint < 0) iint += 4;
                switch (iint)
                {
                    case 0:
                        x = cd;
                        y = 0.0;
                        break;
                    case 1:
                        x = 0.0;
                        y = cd;
                        break;
                    case 2:
                        x = -cd;
                        y = 0.0;
                        break;
                    case 3:
                    default:
                        x = 0.0;
                        y = -cd;
                        break;
                }
            }
            else
            {
                x = Math.Cos(ra * Constants.Degree2Radian) * cd;
                y = Math.Sin(ra * Constants.Degree2Radian) * cd;
            }
            return;
        }

        /// <summary>
        /// Converts a unit vector to (RA, Dec) angles.
        /// </summary>
        /// <param name="p">Input point</param>
        /// <param name="ra">Right Ascension</param>
        /// <param name="dec">Declination</param>
        public static void Cartesian2Radec(Cartesian p, out double ra, out double dec)
        {
            Xyz2Radec(p.x, p.y, p.z, out ra, out dec);
        }

        /// <summary>
        /// Converts a unit vector to (RA, Dec) angles.
        /// </summary>
        /// <param name="p">Input point</param>
        /// <param name="ra">Right Ascension</param>
        /// <param name="dec">Declination</param>
        public static void Cartesian2RadecRadian(Cartesian p, out double ra, out double dec)
        {
            Xyz2RadecRadian(p.x, p.y, p.z, out ra, out dec);
        }

        /// <summary>
        /// Computes the center of mass of specified vectors.
        /// </summary>
        /// <param name="plist">The list of vectors.</param>
        /// <param name="normalize">The flag determines whether the result is normalized.</param>
        public static Cartesian CenterOfMass(IEnumerable<Cartesian> plist, bool normalize)
        {
            Cartesian m = new Cartesian(0, 0, 0, false);
            foreach (Cartesian p in plist)
            {
                m.x += p.x;
                m.y += p.y;
                m.z += p.z;
            }
            if (normalize)
                m.Normalize();
            return m;
        }

#if false
        /// <summary>
        /// Computes the minimal enclosing circle of a list of points.
        /// </summary>
        /// <remarks>
        /// Brute-force algorithm scales with N^4 (means slow).
        /// </remarks>
        public static Halfspace MinimalEnclosingCircleOptimalSlow(IList<Cartesian> plist)
        {
            Halfspace mec = new Halfspace(new Cartesian(1, 0, 0, false), -1, 0);
            double angle, minangle;
            bool found = false;
            minangle = Math.PI;

            double costol, sintol;
            costol = Constants.CosTolerance;
            sintol = Constants.SinTolerance;

            // if a single point -> single point
            if (plist.Count == 1)
                return new Halfspace(plist[0], 1, 0);

            // check diagonals
            Cartesian Pi, Pj, c;
            for (int i = 0; i < plist.Count - 1; i++)
            {
                Pi = plist[i];
                for (int j = i + 1; j < plist.Count; j++)
                {
                    Pj = plist[j];
                    // middle point (normalized, of course)
                    if (Pi.Same(Pj.Mirrored()))
                    {
                        c = Cartesian.CenterOfMass(plist, true);
                    }
                    else
                    {
                        c = plist[i].GetMiddlePoint(plist[j], true);
                    }
                    double rad = c.AngleInRadian(Pj);
                    Halfspace h = new Halfspace(c, Math.Cos(rad), Math.Sin(rad));
                    angle = h.RadiusInRadian;
                    if (angle < minangle && h.Contains(plist, costol, sintol))
                    {
                        found = true;
                        mec = h;
                        minangle = angle;
                    }
                }
            }
            // check triangles
            for (int i = 0; i < plist.Count - 1; i++)
            {
                for (int j = i + 1; j < plist.Count; j++)
                {
                    for (int k = 0; k < plist.Count; k++)
                    {
                        if (k == i || k == j) continue;
                        Halfspace h;
                        try
                        {
                            h = new Halfspace(plist[i], plist[j], plist[k]);
                        }
                        catch (ArgumentException)
                        {
                            continue;
                        }
                        angle = h.RadiusInRadian;
                        if (angle < minangle && h.Contains(plist, costol, sintol))
                        {
                            found = true;
                            mec = h;
                            minangle = angle;
                        }
                        else
                        {
                            h.Invert();
                            angle = h.RadiusInRadian;
                            if (angle < minangle && h.Contains(plist, costol, sintol))
                            {
                                found = true;
                                mec = h;
                                minangle = angle;
                            }
                        }
                    }
                }
            }
            if (!found)
                throw new ArgumentException("MinimalEnclosingCircle(): not found! invalid input?");

            return mec;
        }

        /// <summary>
        /// Computes the minimal enclosing circle of a list of points.
        /// </summary>
        /// <param name="plist"></param>
        /// <returns></returns>
        public static Halfspace MinimalEnclosingCircle(IList<Cartesian> plist)
        {
            Cartesian center;
            double radius;
            FindBoundingSphere(plist, out center, out radius);

            Cartesian n = center;
            double norm2 = n.Norm2;
            double norm = Math.Sqrt(norm2);
            n.Scale(1 / norm);

            double r2 = radius * radius;
            double d = (1 + norm2 - r2) / norm / 2;
            /*
            if (center.Same(new Cartesian())) Console.Error.WriteLine("Center is 0");
            if (norm2 > 1) Console.Error.WriteLine("p2 > 1 !?!");
            if (r2 > 1) Console.Error.WriteLine("r2 > 1 !?!");
            if (d < 0) Console.Error.WriteLine("Hole?");
            */
            if (radius >= 1 || d <= 0)
                return Cartesian.MinimalEnclosingCircleOptimalSlow(plist);

            return new Halfspace(n, d);
        }
#endif

        /// <summary>
        /// Computes the near-optimal bounding sphere of a list of points.
        /// </summary>
        /// <remarks>Based on code by Jack Ritter from Graphics Gems, Academic Press, 1990</remarks>
        /// <param name="plist"></param>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public static void FindBoundingSphere(IList<Cartesian> plist, out Cartesian center, out double radius)
        {
            /*
                An Efficient Bounding Sphere
                by Jack Ritter
                from "Graphics Gems", Academic Press, 1990
            */
            /* Routine to calculate near-optimal bounding sphere over    */
            /* a set of points in 3D */
            /* This contains the routine find_bounding_sphere(), */
            /* the struct definition, and the globals used for parameters. */
            /* The abs() of all coordinates must be < BIGNUMBER */
            /* Code written by Jack Ritter and Lyle Rains. */
            int i;
            double dx, dy, dz;
            double rad_sq, xspan, yspan, zspan, maxspan;
            double old_to_p, old_to_p_sq, old_to_new;
            Cartesian xmin, xmax, ymin, ymax, zmin, zmax, dia1, dia2;

            /* FIRST PASS: find 6 minima/maxima points */

            xmin = ymin = zmin = new Cartesian(double.MaxValue, double.MaxValue, double.MaxValue, false);
            xmax = ymax = zmax = new Cartesian(double.MinValue, double.MinValue, double.MinValue, false);

            for (i = 0; i < plist.Count; i++)
            {
                Cartesian caller_p = plist[i];
                if (caller_p.x < xmin.x) xmin = caller_p;
                if (caller_p.x > xmax.x) xmax = caller_p;
                if (caller_p.y < ymin.y) ymin = caller_p;
                if (caller_p.y > ymax.y) ymax = caller_p;
                if (caller_p.z < zmin.z) zmin = caller_p;
                if (caller_p.z > zmax.z) zmax = caller_p;
            }

            /* Set xspan = distance between the 2 points xmin & xmax (squared) */
            dx = xmax.x - xmin.x;
            dy = xmax.y - xmin.y;
            dz = xmax.z - xmin.z;
            xspan = dx * dx + dy * dy + dz * dz;

            /* Same for y & z spans */
            dx = ymax.x - ymin.x;
            dy = ymax.y - ymin.y;
            dz = ymax.z - ymin.z;
            yspan = dx * dx + dy * dy + dz * dz;

            dx = zmax.x - zmin.x;
            dy = zmax.y - zmin.y;
            dz = zmax.z - zmin.z;
            zspan = dx * dx + dy * dy + dz * dz;

            /* Set points dia1 & dia2 to the maximally separated pair */
            dia1 = xmin; dia2 = xmax; /* assume xspan biggest */
            maxspan = xspan;
            if (yspan > maxspan)
            {
                maxspan = yspan;
                dia1 = ymin; dia2 = ymax;
            }
            if (zspan > maxspan)
            {
                dia1 = zmin; dia2 = zmax;
            }

            /* dia1,dia2 is a diameter of initial sphere */
            /* calc initial center */
            center.x = (dia1.x + dia2.x) / 2.0;
            center.y = (dia1.y + dia2.y) / 2.0;
            center.z = (dia1.z + dia2.z) / 2.0;
            /* calculate initial radius**2 and radius */
            dx = dia2.x - center.x; /* x component of radius vector */
            dy = dia2.y - center.y; /* y component of radius vector */
            dz = dia2.z - center.z; /* z component of radius vector */
            rad_sq = dx * dx + dy * dy + dz * dz;
            radius = Math.Sqrt(rad_sq);

            /* SECOND PASS: increment current sphere */

            for (i = 0; i < plist.Count; i++)
            {
                Cartesian caller_p = plist[i];
                dx = caller_p.x - center.x;
                dy = caller_p.y - center.y;
                dz = caller_p.z - center.z;
                old_to_p_sq = dx * dx + dy * dy + dz * dz;
                if (old_to_p_sq > rad_sq) 	/* do r**2 test first */
                { 	/* this point is outside of current sphere */
                    old_to_p = Math.Sqrt(old_to_p_sq);
                    /* calc radius of new sphere */
                    radius = (radius + old_to_p) / 2.0;
                    rad_sq = radius * radius; 	/* for next r**2 compare */
                    old_to_new = old_to_p - radius;
                    /* calc center of new sphere */
                    center.x = (radius * center.x + old_to_new * caller_p.x) / old_to_p;
                    center.y = (radius * center.y + old_to_new * caller_p.y) / old_to_p;
                    center.z = (radius * center.z + old_to_new * caller_p.z) / old_to_p;
                    /* Suppress if desired */
                    //printf("\n New sphere: cen,rad = %f %f %f   %f",
                    //    cen.x,cen.y,cen.z, rad);
                }
            }
        }

        #endregion
        #region Triangle area computation
#if false
        public static double EuclidArea(Cartesian p1, Cartesian p2, Cartesian p3)
        {
            Cartesian u, w;
            Cartesian c = p1.Add(p2);
            c = c.Add(p3);
            c.Normalize();
            c.Tangent(out w, out u);

            double x1, x2, x3, y1, y2, y3;
            // x from w
            x1 = p1.Dot(w);
            x2 = p2.Dot(w);
            x3 = p3.Dot(w);
            y1 = p1.Dot(u);
            y2 = p2.Dot(u);
            y3 = p3.Dot(u);

            double area = -0.5 * (x1 * y2 + y1 * x3 + y3 * x2 - y2 * x3 - y1 * x2 - x1 * y3);
            area *= Constants.SquareRadian2SquareDegree;
            return area;
        }

        public static double HeronArea(double a1, double a2, double a3)
        {
            double s = 0.5 * (a1 + a2 + a3);
            double ret = Math.Sqrt(s * (s - a1) * (s - a2) * (s - a3));
            return ret;
        }

        /// <summary>
        /// Computes the area of a very elongated triangle with one small and two long arcs.
        /// </summary>
        /// <remarks>Only an approximation, need to add semilune area</remarks>
        /// <param name="?"></param>
        /// <returns></returns>
        public static double SliceArea(double a1, double a2, double a3)
        {
            double[] a = new double[] { a1, a2, a3 };
            Array.Sort<double>(a);
            a1 = a[0];
            a2 = a[1];
            a3 = a[2];

            double aa = 0.5 * (a2 + a3);
            double d = a3 - a2;
            double sd = Math.Sqrt(a1 * a1 - d * d);
            double area = sd * (1 - Math.Cos(aa)) / Math.Sin(aa);

            return area;
        }

        public static double SymmArea(double a, double b)
        {
            /*
            double b2 = b / 2;
            double t0 = 2 * Math.Asin(Math.Sin(b2) / Math.Sin(a));
            t0 -= 2 * Math.Asin(Math.Tan(b2) / Math.Tan(a));
            return t0;
            */
            double b2 = b / 2;
            double alpha = Math.Asin(Math.Sin(b2) / Math.Sin(a));
            double beta = Math.Asin(Math.Tan(b2) / Math.Tan(a));

            double t = 2 * (alpha - beta);
            return t;
        }

        public static double SymmSmallArea(double a, double b)
        {
            double b2 = b / 2;
            double alpha = Math.Asin(Math.Sin(b2) / Math.Sin(a));
            double beta = Math.Asin(Math.Tan(b2) / Math.Tan(a));

            double t = 4 * Math.Asin(b2 / a / 12 * (b2 * b2 - a * a) / Math.Cos(0.5 * (alpha + beta)));
            return t;
        }

        public static double SymmArea2(double a, double b)
        {
            double a2 = a / 2;
            double b2 = b / 2;
            double z = Math.Tan(a2);
            //z *= z;
            z *= Math.Tan(b2);
            z *= Math.Sqrt(Math.Sin(a - b2) * Math.Sin(a + b2)) / Math.Sin(a);
            double t = 2 * Math.Asin(z);
            return t;
        }

        public static double RecursiveArea(double a, double b, double c)
        {
            SortDoubleDesc(ref a, ref b, ref c);
            double s = 0.5 * (a + b + c);
            
            double area;

            // if small, heron area, else recurse with isoceles
            if (a < 1e-4)
            {
                area = Math.Sqrt(s * (s - a) * (s - b) * (s - c));
            }
            else
            {
                // split it
                double q = Math.Sin(b) * Math.Sin(s - a) * Math.Sin(s - b) / Math.Sin(a);
                double d = 2 * Math.Asin(Math.Sqrt(q));
                // area of iso
                area = SymmArea2(b, d);
                // and the rest if needed
                if (Math.Abs(a - b) > 1e-10)
                    area += RecursiveArea(a - b, d, c);
            }
            return area;
        }

                private static void SortDoubleDesc(ref double a, ref double b, ref double c)
        {
            // sort them
            double[] v = new double[] { a, b, c };
            Array.Sort<double>(v);
            c = v[0]; // smallest
            b = v[1];
            a = v[2]; // largest
        }

        public static double GirardArea(double a1, double a2, double a3)
        {
            double s = 0.5 * (a1 + a2 + a3);
            double area = -Math.PI
    + 2 * Math.Asin(Math.Sqrt(Math.Sin(s - a2) * Math.Sin(s - a3) / (Math.Sin(a2) * Math.Sin(a3))))
    + 2 * Math.Asin(Math.Sqrt(Math.Sin(s - a1) * Math.Sin(s - a3) / (Math.Sin(a1) * Math.Sin(a3))))
    + 2 * Math.Asin(Math.Sqrt(Math.Sin(s - a2) * Math.Sin(s - a1) / (Math.Sin(a2) * Math.Sin(a1))))
    ;
            return area;
        }

                /// <summary>
        /// Computes the area of the triangle defined by three points connected with great circles.
        /// </summary>
        /// <param name="p1">Input unit vector</param>
        /// <param name="p2">Input unit vector</param>
        /// <param name="p3">Input unit vector</param>
        /// <returns>The area</returns>
        public static double SphericalTriangleArea_Prev(Cartesian p1, Cartesian p2, Cartesian p3)
        {
            //
            // Girard's theorem
            //
            double area = 0, det, a1 = 0, a2 = 0, a3 = 0, dx, dy, dz;
            Cartesian v1 = new Cartesian(p1.x - p3.x, p1.y - p3.y, p1.z - p3.z, false);
            Cartesian v2 = new Cartesian(p2.x - p3.x, p2.y - p3.y, p2.z - p3.z, false);
            Cartesian v3 = p3;

            det = Cartesian.TripleProduct(v3, v1, v2);
            if (Math.Abs(det) < 1e-15) return 0;

            a1 = 2 * Math.Asin(0.5 * Math.Sqrt(v2.x * v2.x + v2.y * v2.y + v2.z * v2.z));
            a2 = 2 * Math.Asin(0.5 * Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z));
            dx = v1.x - v2.x;
            dy = v1.y - v2.y;
            dz = v1.z - v2.z;
            a3 = 2 * Math.Asin(0.5 * Math.Sqrt(dx * dx + dy * dy + dz * dz));

            double mina = Math.Min(a1, Math.Min(a2, a3));
            double maxa = Math.Max(a1, Math.Max(a2, a3));

            if (mina < 1e-3)
            {
                if (maxa < 1e-3)
                {
                    // small-small: euclid
                    area = Cartesian.HeronArea(a1, a2, a3); //EuclidArea(p1, p2, p3);                    
                }
                else
                {
                    // small-large: slice
                    area = Cartesian.SliceArea(a1, a2, a3);
                }
            }
            else
            {
                // large-large: girard
                area = Cartesian.GirardArea(a1, a2, a3);
            }

            area *= Constants.SquareRadian2SquareDegree;
            if (det < 0) area *= -1;
            return area;
        }
#endif

        /// <summary>
        /// Computes the Girard area.
        /// </summary>
        /// <remarks>It uses the most robust formula.</remarks>
        /// <param name="a">The first side of the triangle in radians.</param>
        /// <param name="b">The second side of the triangle in radians.</param>
        /// <param name="c">The third side of the triangle in radians.</param>
        /// <returns>The area in steradians.</returns>
        public static double GirardArea(double a, double b, double c)
        {
            if (a < 0 || b < 0 || c < 0)
            {
                throw new ArgumentOutOfRangeException
                    ("Cartesian.GirardArea(): negative arc length?");
            }
            double s = 0.5 * (a + b + c);

            double sa = s - a;
            double sb = s - b;
            double sc = s - c;

            double t;
            if (sa <= 0 || sb <= 0 || sc <= 0
                || sa / s < Constants.DoublePrecision2x
                || sb / s < Constants.DoublePrecision2x
                || sc / s < Constants.DoublePrecision2x)
            {
                t = 0;
            }
            else
            {
                double z = Math.Tan(s / 2)
                    * Math.Tan(sa / 2)
                    * Math.Tan(sb / 2)
                    * Math.Tan(sc / 2);

                t = 4 * Math.Atan(Math.Sqrt(z));
            }
            return t;
        }



        /// <summary>
        /// Computes the area of the triangle defined by three points connected with great circles.
        /// </summary>
        /// <param name="p1">Input unit vector</param>
        /// <param name="p2">Input unit vector</param>
        /// <param name="p3">Input unit vector</param>
        /// <returns>The area</returns>
        public static double SphericalTriangleArea(Cartesian p1, Cartesian p2, Cartesian p3)
        {
            // difference vectors
            Cartesian v1 = new Cartesian(p3.x - p2.x, p3.y - p2.y, p3.z - p2.z, false);
            Cartesian v2 = new Cartesian(p1.x - p3.x, p1.y - p3.y, p1.z - p3.z, false);
            Cartesian v3 = new Cartesian(p2.x - p1.x, p2.y - p1.y, p2.z - p1.z, false);
            // related angles
            double a1 = 2 * Math.Asin(0.5 * Math.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z));
            double a2 = 2 * Math.Asin(0.5 * Math.Sqrt(v2.x * v2.x + v2.y * v2.y + v2.z * v2.z));
            double a3 = 2 * Math.Asin(0.5 * Math.Sqrt(v3.x * v3.x + v3.y * v3.y + v3.z * v3.z));

            double area;
            double det = Cartesian.TripleProduct(v1, v2, p3);

            if (Math.Abs(det) < double.Epsilon)
            {
                area = 0;
            }
            else
            {
                area = GirardArea(a1, a2, a3);
                area *= Constants.SquareRadian2SquareDegree;
                if (det < 0) area *= -1;
            }
            return area;
        }

        #endregion
        #region Predefined readonly points

        /// <summary>
        /// Is a vector of NaN coordinates.
        /// </summary>
        public static readonly Cartesian NaN = new Cartesian(double.NaN, double.NaN, double.NaN, false);

        /// <summary>
        /// Is the unit vector of the X axis: (1,0,0).
        /// </summary>
        public static readonly Cartesian Xaxis = new Cartesian(1, 0, 0, false);
        /// <summary>
        /// Is the unit vector of the Y axis: (0,1,0).
        /// </summary>
        public static readonly Cartesian Yaxis = new Cartesian(0, 1, 0, false);
        /// <summary>
        /// Is the unit vector of the Z axis: (0,0,1).
        /// </summary>
        public static readonly Cartesian Zaxis = new Cartesian(0, 0, 1, false);

        #endregion
        #region Equality related methods...

        /// <summary>
        /// Determines whether the the vector is equal to the specified vector.
        /// </summary>
        public bool Equals(Cartesian right)
        {
            return this.x.Equals(right.x) && this.y.Equals(right.y) && this.z.Equals(right.z);
        }

        /// <summary>
        /// Determines whether the vector is equal to the specified object.
        /// </summary>
        public override bool Equals(object right)
        {
            if (right == null)
                return false;

            if (object.ReferenceEquals(this, right))
                return true;

            if (this.GetType() != right.GetType())
                return false;

            Cartesian p = (Cartesian)right;
            return this.Equals(p);
        }

        /// <summary>
        /// Provides hashcode for the vector.
        /// </summary>
        public override int GetHashCode()
        {
#if true
            double x, y, z;
            double f = Constants.HashMagic;
            x = Math.Round(this.x, Constants.HashDigit) * f; f *= f;
            y = Math.Round(this.y, Constants.HashDigit) * f; f *= f;
            z = Math.Round(this.z, Constants.HashDigit) * f;
            int hashcode = x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
            return hashcode;
#else
            //return this.ToString().GetHashCode();
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
#endif
        }

        /// <summary>
        /// Determines whether two vectors are the same.
        /// </summary>
        public static bool operator ==(Cartesian left, Cartesian right)
        {
            return (left.x == right.x) && (left.y == right.y) && (left.z == right.z);
        }

        /// <summary>
        /// Determines whether two vectors are different.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static bool operator !=(Cartesian left, Cartesian right)
        {
            return (left.x != right.x) || (left.y != right.y) || (left.z != right.z);
        }

        /// <summary>
        /// Determines whether the coordinates of the vectors are NaN.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsNaN(Cartesian p)
        {
            return double.IsNaN(p.x) && double.IsNaN(p.y) && double.IsNaN(p.z);
        }

        #endregion
        #region Parsing, ToString and revision

        /// <summary>
        /// Parses an input string of "x y z" into a Cartesian 
        /// </summary>
        /// <param name="repr"></param>
        /// <param name="normalize"></param>
        /// <returns></returns>
        public static Cartesian Parse(string repr, bool normalize)
        {
            char[] sep = new char[] { ' ' };
#if false
            // only spaces after this (hopefully)
            string proc = repr.Trim().Replace(',', ' ').Replace("\r\n", " ").Replace('\n', ' ').Replace('\t', ' ');

            // replace duplicate spaces with a single one
            string trim = "";
            while (trim != proc)
            {
                trim = proc;
                proc = trim.Replace("  ", " ");
            }
            string[] t = trim.Split(sep, 3);
#else
            string[] t = repr.Split(sep, 3);
#endif
            double x = double.Parse(t[0]);
            double y = double.Parse(t[1]);
            double z = double.Parse(t[2]);
            return new Cartesian(x, y, z, normalize);
        }

        /// <summary>
        /// String representation is the three coordinates: x y z
        /// </summary>
        /// <remarks>Coordinates are written with a precision to reconstruct the same binary representation when read back in</remarks>
#if true
        public override string ToString()
        {
            return String.Format("{0:R} {1:R} {2:R}", x, y, z);
        }
#else
        public override string ToString()
        {
            double ra, dec;
            Cartesian2Radec(this, out ra, out dec);
            return String.Format("{0:R} {1:R} {2:R}\t  {3:R} {4:R}", x,y,z, ra, dec);
        }
#endif
        /// <summary>
        /// Revision from CVS
        /// </summary>
        public static readonly string Revision = "$Revision: 5928 $";

        #endregion
    }
}
