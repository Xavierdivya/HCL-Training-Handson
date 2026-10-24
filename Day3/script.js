const form = document.getElementById('bmiForm');
const weightInput = document.getElementById('weight');
const heightInput = document.getElementById('height');
const weightLabel = document.getElementById('weightLabel');
const heightLabel = document.getElementById('heightLabel');
const errorEl = document.getElementById('error');
const resultEl = document.getElementById('result');
const valueEl = document.getElementById('bmiValue');
const categoryEl = document.getElementById('bmiCategory');
const unitButtons = document.querySelectorAll('.unit');

let unit = 'metric';

// Switch between metric and imperial units
unitButtons.forEach(function (btn) {
  btn.addEventListener('click', function () {
    unit = btn.dataset.unit;
    unitButtons.forEach(function (b) {
      const active = b === btn;
      b.classList.toggle('active', active);
      b.setAttribute('aria-pressed', String(active));
    });
    weightLabel.textContent = unit === 'metric' ? 'Weight (kg)' : 'Weight (lb)';
    heightLabel.textContent = unit === 'metric' ? 'Height (cm)' : 'Height (in)';
    weightInput.placeholder = unit === 'metric' ? 'e.g. 60' : 'e.g. 132';
    heightInput.placeholder = unit === 'metric' ? 'e.g. 165' : 'e.g. 65';
    clearResult();
  });
});

function getCategory(bmi) {
  if (bmi < 18.5) return { text: 'Underweight', cls: 'under' };
  if (bmi < 25) return { text: 'Normal weight', cls: 'normal' };
  if (bmi < 30) return { text: 'Overweight', cls: 'over' };
  return { text: 'Obese', cls: 'obese' };
}

function clearResult() {
  errorEl.textContent = '';
  resultEl.hidden = true;
  resultEl.className = 'result';
}

form.addEventListener('submit', function (e) {
  e.preventDefault();
  clearResult();

  const weight = parseFloat(weightInput.value);
  const height = parseFloat(heightInput.value);

  if (!(weight > 0) || !(height > 0)) {
    errorEl.textContent = 'Please enter a weight and height greater than 0.';
    return;
  }

  // Metric: kg / m^2.  Imperial: 703 * lb / in^2.
  const bmi = unit === 'metric'
    ? weight / Math.pow(height / 100, 2)
    : (703 * weight) / Math.pow(height, 2);

  if (!isFinite(bmi) || bmi < 5 || bmi > 100) {
    errorEl.textContent = 'These values look unrealistic. Please check them.';
    return;
  }

  const category = getCategory(bmi);
  valueEl.textContent = bmi.toFixed(1);
  categoryEl.textContent = category.text;
  resultEl.className = 'result ' + category.cls;
  resultEl.hidden = false;
});

document.getElementById('resetBtn').addEventListener('click', function () {
  form.reset();
  clearResult();
  weightInput.focus();
});
