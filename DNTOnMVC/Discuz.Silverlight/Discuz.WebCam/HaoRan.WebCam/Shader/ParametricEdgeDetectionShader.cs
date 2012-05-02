//
//   Project:           Shaders
//
//   Description:       Pixel shader: Edge detection using a parametric, symetric, directional convolution kernel.
//
//   Changed by:        $Author: Rene $
//   Changed on:        $Date: 2009-11-19 23:57:25 +0100 (Do, 19 Nov 2009) $
//   Changed in:        $Revision: 67 $
//   Project:           $URL: file:///U:/Data/Development/SVN/SilverlightDemos/trunk/EdgeCam/EdgeCam/Shader/ParametricEdgeDetectionShader.cs $
//   Id:                $Id: ParametricEdgeDetectionShader.cs 67 2009-11-19 22:57:25Z Rene $
//
//
//   Copyright (c) 2009 Rene Schulte
//
//   This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License 
//   as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version.
//   This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
//   without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
//   See the GNU General Public License for more details.
//   You should have received a copy of the GNU General Public License along with this program; ("License.txt").
//   if not, see <http://www.gnu.org/licenses/>. 
//

using System;
using System.Windows;
using System.Windows.Media.Effects;

namespace HaoRan.WebCam
{
   public class ParametricEdgeDetectionShader : ShaderEffect
   {
      #region DependencyProperties

      public static DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(ParametricEdgeDetectionShader), 0);
      public static DependencyProperty ThresholdProperty = DependencyProperty.Register("Threshold", typeof(float), typeof(ParametricEdgeDetectionShader), new PropertyMetadata(0.5f, ShaderEffect.PixelShaderConstantCallback(0)));
      public static DependencyProperty K00Property = DependencyProperty.Register("K00", typeof(float), typeof(ParametricEdgeDetectionShader), new PropertyMetadata(1f, ShaderEffect.PixelShaderConstantCallback(1)));
      public static DependencyProperty K01Property = DependencyProperty.Register("K01", typeof(float), typeof(ParametricEdgeDetectionShader), new PropertyMetadata(2f, ShaderEffect.PixelShaderConstantCallback(2)));
      public static DependencyProperty K02Property = DependencyProperty.Register("K02", typeof(float), typeof(ParametricEdgeDetectionShader), new PropertyMetadata(1f, ShaderEffect.PixelShaderConstantCallback(3)));
      public static DependencyProperty TextureSizeProperty = DependencyProperty.Register("TextureSize", typeof(System.Windows.Point), typeof(ParametricEdgeDetectionShader), new PropertyMetadata(new System.Windows.Point(512, 512), ShaderEffect.PixelShaderConstantCallback(4)));
      
      #endregion

      #region Properties

      public virtual System.Windows.Media.Brush Input
      {
         get
         {
            return ((System.Windows.Media.Brush)(GetValue(InputProperty)));
         }
         set
         {
            SetValue(InputProperty, value);
         }
      }

      public virtual float Threshold
      {
         get
         {
            return ((float)(GetValue(ThresholdProperty)));
         }
         set
         {
            SetValue(ThresholdProperty, value);
         }
      }

      public virtual float K00
      {
         get
         {
            return ((float)(GetValue(K00Property)));
         }
         set
         {
            SetValue(K00Property, value);
         }
      }

      public virtual float K01
      {
         get
         {
            return ((float)(GetValue(K01Property)));
         }
         set
         {
            SetValue(K01Property, value);
         }
      }

      public virtual float K02
      {
         get
         {
            return ((float)(GetValue(K02Property)));
         }
         set
         {
            SetValue(K02Property, value);
         }
      }

      public virtual System.Windows.Point TextureSize
      {
         get
         {
            return ((System.Windows.Point)(GetValue(TextureSizeProperty)));
         }
         set
         {
            SetValue(TextureSizeProperty, value);
         }
      }

      #endregion

      #region Contructors

      public ParametricEdgeDetectionShader()
      {
          this.PixelShader = new PixelShader() { UriSource = new Uri("/HaoRan.WebCam;component/Shader/ParametricEdgeDetection.ps", UriKind.Relative) };
         this.UpdateShaderValue(InputProperty);
         this.UpdateShaderValue(ThresholdProperty);
         this.UpdateShaderValue(K00Property);
         this.UpdateShaderValue(K01Property);
         this.UpdateShaderValue(K02Property);
         this.UpdateShaderValue(TextureSizeProperty);
      }

      #endregion   
   }
}